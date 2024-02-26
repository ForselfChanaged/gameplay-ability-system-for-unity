using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
namespace GAS.Editor.Ability.AbilityTimelineEditor.Track
{
    public abstract class TrackBase
    {
        protected float FrameWidth;
        protected VisualElement MenuParent;
        protected VisualElement TrackParent;
        protected VisualElement Track;
        protected VisualElement Menu;
        
        protected abstract string TrackAssetPath { get; }
        protected abstract string MenuAssetPath { get; }
        public abstract void TickView(int frameIndex, params object[] param);
        public abstract bool CheckFrameIndexOnDrag(int targetIndex);
        public abstract void SetFrameIndex(int oldIndex, int newIndex);
        

        // public TrackBase(VisualElement trackParent, VisualElement menuParent, float frameWidth)
        // {
        //     Init(trackParent, menuParent,frameWidth);
        // }

        public virtual void Init( VisualElement trackParent, VisualElement menuParent, float frameWidth)
        {
            TrackParent = trackParent;
            MenuParent = menuParent;
            Track = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(TrackAssetPath).Instantiate().Query().ToList()[1];
            Menu = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(MenuAssetPath).Instantiate().Query().ToList()[1];
            TrackParent.Add(Track);
            MenuParent.Add(Menu);
            
            FrameWidth = frameWidth;
        }

        public virtual void RefreshShow(float newFrameWidth)
        {
            FrameWidth = newFrameWidth;
        }

        public virtual void RefreshShow()
        {
            RefreshShow(FrameWidth);
        }

        #region Select

        public void Select(){}
        public void OnSelect(){}
        public void OnUnSelect(){}

        #endregion
    }
}
#endif