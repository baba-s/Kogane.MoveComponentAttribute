using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kogane.Internal
{
    /// <summary>
    /// MoveComponentUpAttribute / MoveComponentDownAttribute 付きのコンポーネントの位置を移動するエディタ拡張
    /// </summary>
    internal static class MoveComponent
    {
        //================================================================================
        // 関数(static)
        //================================================================================
        /// <summary>
        /// スクリプトがコンパイルされた時に呼び出されます
        /// </summary>
        [DidReloadScripts]
        private static void OnReloadScripts()
        {
            EditorSceneManager.sceneOpened         -= OnSceneOpened;
            EditorSceneManager.sceneOpened         += OnSceneOpened;
            EditorSceneManager.sceneSaving         -= OnSceneSaving;
            EditorSceneManager.sceneSaving         += OnSceneSaving;
            ObjectFactory.componentWasAdded        -= OnComponentWasAdded;
            ObjectFactory.componentWasAdded        += OnComponentWasAdded;
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

            MoveAll();
        }

        /// <summary>
        /// シーンファイルを開いた時に呼び出されます
        /// </summary>
        private static void OnSceneOpened( Scene scene, OpenSceneMode mode )
        {
            if ( EditorApplication.isPlaying ) return;

            MoveAll();
        }

        /// <summary>
        /// シーンファイルが保存される時に呼び出されます
        /// </summary>
        private static void OnSceneSaving( Scene scene, string path )
        {
            if ( EditorApplication.isPlaying ) return;

            MoveAll();
        }

        /// <summary>
        /// コンポーネントがアタッチされた時に呼び出されます
        /// </summary>
        private static void OnComponentWasAdded( Component component )
        {
            if ( EditorApplication.isPlaying ) return;

            MoveAll();
        }

        /// <summary>
        /// Unity を再生する時に呼び出されます
        /// </summary>
        private static void OnPlayModeStateChanged( PlayModeStateChange change )
        {
            if ( change != PlayModeStateChange.ExitingEditMode ) return;

            MoveAll();
        }

        /// <summary>
        /// シーンに存在するすべての
        /// MoveComponentUpAttribute / MoveComponentDownAttribute 付きのコンポーネントの位置を移動します
        /// </summary>
        private static void MoveAll()
        {
            if ( EditorApplication.isPlaying ) return;

            var monoBehaviours = GetAllMonoBehaviour();

            foreach ( var monoBehaviour in monoBehaviours )
            {
                MoveUp( monoBehaviour );
                MoveDown( monoBehaviour );
            }
        }

        /// <summary>
        /// 指定された MonoBehaviour が MoveComponentUpAttribute 付きであれば位置を移動します
        /// </summary>
        private static void MoveUp( MonoBehaviour monoBehaviour )
        {
            var type      = monoBehaviour.GetType();
            var attribute = type.GetCustomAttribute<MoveComponentUpAttribute>();

            if ( attribute == null ) return;

            var components = monoBehaviour.GetComponents<Component>();
            var component  = Array.Find( components, x => x.GetType() == attribute.Type || x.GetType().IsInherits( attribute.Type ) );

            if ( component == null ) return;

            var currentIndex = Array.IndexOf( components, monoBehaviour );
            var targetIndex  = Array.IndexOf( components, component );

            while ( currentIndex < targetIndex - 1 )
            {
                ComponentUtility.MoveComponentDown( monoBehaviour );
                currentIndex++;
            }

            while ( targetIndex < currentIndex )
            {
                ComponentUtility.MoveComponentUp( monoBehaviour );
                currentIndex--;
            }
        }

        /// <summary>
        /// 指定された MonoBehaviour が MoveComponentDownAttribute 付きであれば位置を移動します
        /// </summary>
        private static void MoveDown( MonoBehaviour monoBehaviour )
        {
            var type      = monoBehaviour.GetType();
            var attribute = type.GetCustomAttribute<MoveComponentDownAttribute>();

            if ( attribute == null ) return;

            var components = monoBehaviour.GetComponents<Component>();
            var component  = Array.Find( components, x => x.GetType() == attribute.Type || x.GetType().IsInherits( attribute.Type ) );

            if ( component == null ) return;

            var currentIndex = Array.IndexOf( components, monoBehaviour );
            var targetIndex  = Array.IndexOf( components, component );

            while ( currentIndex < targetIndex )
            {
                ComponentUtility.MoveComponentDown( monoBehaviour );
                currentIndex++;
            }

            while ( targetIndex + 1 < currentIndex )
            {
                ComponentUtility.MoveComponentUp( monoBehaviour );
                currentIndex--;
            }
        }

        /// <summary>
        /// シーンに存在するすべての MonoBehaviour を返します
        /// </summary>
        private static MonoBehaviour[] GetAllMonoBehaviour()
        {
            return Resources
                    .FindObjectsOfTypeAll<MonoBehaviour>()
                    .Where( x => x.gameObject.scene.isLoaded )
                    .Where( x => x.gameObject.hideFlags == HideFlags.None )
                    .Where( x => PrefabUtility.GetPrefabAssetType( x.gameObject ) == PrefabAssetType.NotAPrefab )
                    .ToArray()
                ;
        }
    }
}