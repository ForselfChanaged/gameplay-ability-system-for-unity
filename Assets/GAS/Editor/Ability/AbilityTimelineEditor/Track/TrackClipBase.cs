﻿using GAS.Runtime.Ability.AbilityTimeline;
using UnityEngine;
using UnityEngine.UIElements;

namespace GAS.Editor.Ability.AbilityTimelineEditor.Track
{
    public abstract class TrackClipBase
    {
        protected TrackClipVisualElement ve;
        public TrackClipVisualElement Ve => ve;
        public float FrameUnitWidth { get;protected set; }
        public int StartFrameIndex=>clipData.startFrame;
        public int EndFrameIndex=>clipData.EndFrame;
        public int DurationFrame => clipData.durationFrame;
        protected ClipEventBase clipData;
        protected TrackBase trackBase;
        public TrackBase TrackBase => trackBase;
        public ClipEventBase ClipData => clipData;
        
        public virtual void InitTrackClip(
            TrackBase track,
            VisualElement parent,
            float frameUnitWidth,
            ClipEventBase clipData)
        {
            trackBase = track;
            FrameUnitWidth = frameUnitWidth;
            this.clipData = clipData;
            
            ve = new TrackClipVisualElement();
            ve.InitClipInfo(this);
            parent.Add(ve);
            if(AbilityTimelineEditorWindow.Instance.CurrentInspectorObject is TrackClipBase clipBase &&
               clipData == clipBase.clipData)
                ve.OnSelect();
            else
                ve.OnUnSelect();
        }

        public virtual VisualElement Inspector()
        {
            return null;
        }

        public virtual void Delete()
        {
        }

        public virtual void RefreshShow(float newFrameUnitWidth)
        {
            FrameUnitWidth = newFrameUnitWidth;
        }

        protected void CheckStyle(VisualElement element)
        {
            element.style.fontSize = 14;
            //element.style.width = new StyleLength(1);
        }

        public void UpdateClipDataStartFrame(int newStartFrame)
        {
            clipData.startFrame = newStartFrame;
        }
        
        public void UpdateClipDataDurationFrame(int neDurationFrame)
        {
            clipData.durationFrame = neDurationFrame;
        }
        
        #region Visual Element Event
        
        public void OnMainMouseDown(MouseDownEvent evt)
        {
        }

        public void OnMainMouseUp(MouseUpEvent evt)
        {
        }

        public void OnMainMouseMove(MouseMoveEvent evt)
        {
        }

        public void OnMainMouseOut(MouseOutEvent evt)
        {
        }
        
        #endregion
    }
    
    public abstract class TrackClip<T>:TrackClipBase where T : TrackBase
    {
        protected T track;

        public override void InitTrackClip(
            TrackBase track,
            VisualElement parent,
            float frameUnitWidth,
            ClipEventBase clipData)
        {
            this.track = (T) track;
            base.InitTrackClip(track, parent, frameUnitWidth, clipData);
            
            RefreshShow(FrameUnitWidth);
        }
       
    }

}