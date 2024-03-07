using System;
using GAS.Runtime.Ability.TimelineAbility;
using UnityEngine;
using UnityEngine.UIElements;

namespace GAS.Editor.Ability.AbilityTimelineEditor
{
    public class MenuTrack : TrackBase
    {
        private static TimelineAbilityAsset AbilityAsset => AbilityTimelineEditorWindow.Instance.AbilityAsset;
        private static AbilityTimelineEditorConfig Config => AbilityTimelineEditorWindow.Instance.Config;
        private static TimelineTrackView TrackView => AbilityTimelineEditorWindow.Instance.TrackView;
        
        private Color _trackColor;
        private Color _menuColor;
        public override Type TrackDataType { get; }
        protected override Color TrackColor => _trackColor;
        protected override Color MenuColor => _menuColor;
        protected override string MenuAssetGuid => "944173d62639bb04b8b64be960c8ef29";
        
        public Button AddButton { get; private set; }
        private Type _trackType;
        private Type _trackDataType;

        public void Init(VisualElement trackParent, VisualElement menuParent, float frameWidth, Type trackType, Type trackDataType,
            string label, Color trackColor, Color menuColor)
        {
            _trackType = trackType;
            _trackDataType = trackDataType;
            _trackColor = trackColor;
            _menuColor = menuColor;
            
            base.Init(trackParent, menuParent, frameWidth, null);
            AddButton = MenuRoot.Q<Button>("BtnAdd");
            AddButton.style.display = DisplayStyle.Flex;
            AddButton.clickable.clicked += OnClickAddTrack;
            MenuBox.style.left = 0;
            MenuBox.style.right = new StyleLength(StyleKeyword.Auto);
            MenuText.text = label;

            Track.style.backgroundColor = new Color(0, 0, 0, 0);
            BoundingBox.style.backgroundColor = MenuColor;
            const int height = 20;
            MenuRoot.style.height = height;
            MenuRoot.style.minHeight = height;
            MenuRoot.style.maxHeight = height;
            
            TrackRoot.style.height = height;
            TrackRoot.style.minHeight = height;
            TrackRoot.style.maxHeight = height;
        }
        
        private void OnClickAddTrack()
        {
            // 创建View
            var track = (TrackBase)Activator.CreateInstance(_trackType);
            if (track.IsFixedTrack()) return;

            // 创建Data
            var data = (TrackDataBase)Activator.CreateInstance(_trackDataType);
            data.DefaultInit();
            data.AddToAbilityAsset(AbilityAsset);

            // 初始化View
            track.Init(TrackParent, MenuParent, Config.FrameUnitWidth, data);
            TrackParent.Remove(track.TrackRoot);
            MenuParent.Remove(track.MenuRoot);
            int index = CurrentAddIndex();
            TrackParent.Insert(index,track.TrackRoot);
            MenuParent.Insert(index,track.MenuRoot);
            
            TrackView.TrackList.Add(track);

            Debug.Log("[EX] Add a new track:" + _trackType.Name);

            AbilityAsset.Save();
        }

        public override void TickView(int frameIndex, params object[] param)
        {
        }

        public override VisualElement Inspector()
        {
            return new VisualElement();
        }

        protected override void OnAddTrackItem(DropdownMenuAction action)
        {
        }

        protected override void OnRemoveTrack(DropdownMenuAction action)
        {
        }

        int CurrentAddIndex()
        {
            int baseIndex = TrackParent.IndexOf(TrackRoot);
            if (_trackType == typeof(InstantCueTrack))
                return baseIndex + (AbilityAsset.InstantCues?.Count ?? 0);

            if (_trackType == typeof(TaskMarkEventTrack))
                return baseIndex + (AbilityAsset.InstantTasks?.Count ?? 0);

            if (_trackType == typeof(ReleaseGameplayEffectTrack))
                return baseIndex + (AbilityAsset.ReleaseGameplayEffect?.Count ?? 0);

            if (_trackType == typeof(BuffGameplayEffectTrack))
                return baseIndex + (AbilityAsset.BuffGameplayEffects?.Count ?? 0);

            if (_trackType == typeof(TaskClipEventTrack))
                return baseIndex + (AbilityAsset.OngoingTasks?.Count ?? 0);

            if (_trackType == typeof(DurationalCueTrack))
                return baseIndex + (AbilityAsset.DurationalCues?.Count ?? 0);

            return -1;
        }
    }
}