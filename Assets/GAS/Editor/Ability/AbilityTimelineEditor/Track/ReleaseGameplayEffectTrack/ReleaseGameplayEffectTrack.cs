﻿using System;
using System.Collections.Generic;
using GAS.Runtime.Ability.TimelineAbility;
using GAS.Runtime.Effects;
using UnityEngine;
using UnityEngine.UIElements;

namespace GAS.Editor.Ability.AbilityTimelineEditor
{
    public class ReleaseGameplayEffectTrack : TrackBase
    {
        private static TimelineAbilityAsset AbilityAsset => AbilityTimelineEditorWindow.Instance.AbilityAsset;
        public ReleaseGameplayEffectTrackData ReleaseGameplayEffectTrackData {
            get
            {
                for (int i = 0; i < AbilityAsset.ReleaseGameplayEffect.Count; i++)
                {
                    if(AbilityAsset.ReleaseGameplayEffect[i] == _releaseGameplayEffectTrackData)
                        return AbilityAsset.ReleaseGameplayEffect[i];
                }
                return null;
            }
        }

        private ReleaseGameplayEffectTrackData _releaseGameplayEffectTrackData;
        public override Type TrackDataType => typeof(ReleaseGameplayEffectTrackData);
        protected override Color TrackColor => new(0.9f, 0.3f, 0.35f, 0.2f);
        protected override Color MenuColor => new(0.9f, 0.3f, 0.35f, 0.9f);

        public override void Init(VisualElement trackParent, VisualElement menuParent, float frameWidth,
            TrackDataBase trackData)
        {
            base.Init(trackParent, menuParent, frameWidth, trackData);
            MenuText.text = "施放型GameplayEffect";
            _releaseGameplayEffectTrackData = trackData as ReleaseGameplayEffectTrackData;
        }

        public override void TickView(int frameIndex, params object[] param)
        {
            foreach (var item in _trackItems)
                ((TrackMarkBase)item).OnTickView(frameIndex);
        }

        public override void RefreshShow(float newFrameWidth)
        {
            base.RefreshShow(newFrameWidth);
            foreach (var item in _trackItems) Track.Remove(item.Ve);
            _trackItems.Clear();

            if (AbilityTimelineEditorWindow.Instance.AbilityAsset == null) return;

            foreach (var markEvent in _releaseGameplayEffectTrackData.markEvents)
            {
                var item = new ReleaseGameplayEffectMark();
                item.InitTrackMark(this, Track, _frameWidth, markEvent);
                _trackItems.Add(item);
            }
        }

        public override VisualElement Inspector()
        {
            var inspector = TrackInspectorUtil.CreateTrackInspector();

            var trackLabel = TrackInspectorUtil.CreateLabel("施放型GameplayEffect:");
            trackLabel.style.fontSize = 14;
            inspector.Add(trackLabel);

            foreach (var mark in _releaseGameplayEffectTrackData.markEvents)
            {
                var markFrame =
                    TrackInspectorUtil.CreateLabel(
                        $"||标记帧:{mark.startFrame}  GameplayEffect数量{mark.gameplayEffectAssets.Count}");
                inspector.Add(markFrame);
                foreach (var ge in mark.gameplayEffectAssets)
                {
                    var geName = ge != null ? ge.name : "NULL";
                    var geNameLabel = TrackInspectorUtil.CreateLabel($"    |-> GameplayEffect:{geName}");
                    inspector.Add(geNameLabel);
                }
            }

            return inspector;
        }

        protected override void OnAddTrackItem(DropdownMenuAction action)
        {
            // 添加Mark数据
            var markEvent = new ReleaseGameplayEffectMarkEvent
            {
                startFrame = GetTrackIndexByMouse(action.eventInfo.localMousePosition.x),
                gameplayEffectAssets = new List<GameplayEffectAsset>()
            };
            ReleaseGameplayEffectTrackData.markEvents.Add(markEvent);

            // 刷新显示
            var mark = new ReleaseGameplayEffectMark();
            mark.InitTrackMark(this, Track, _frameWidth, markEvent);
            _trackItems.Add(mark);

            // 选中新Clip
            mark.OnSelect();

            Debug.Log("[EX] Add ReleaseGameplayEffect Mark");
        }

        protected override void OnRemoveTrack(DropdownMenuAction action)
        {
            // 删除数据
            AbilityAsset.ReleaseGameplayEffect.Remove(_releaseGameplayEffectTrackData);
            AbilityTimelineEditorWindow.Instance.Save();
            // 删除显示
            TrackParent.Remove(TrackRoot);
            MenuParent.Remove(MenuRoot);
            Debug.Log("[EX] Remove Release GameplayEffect Track");
        }
    }
}