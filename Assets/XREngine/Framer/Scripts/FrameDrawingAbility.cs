using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;
using XRCORE.Scripts.VR.Player;
using XREngine.Core.Scripts.VR.Player;
using XREngine.Core.Scripts.VR.Player.Abilities;

namespace XREngine.Framer.Scripts
{
    [Serializable]
    public class DrawingParent
    {
        public bool Shown;
        public List<CurvedLineRenderer> LinesDrawn = new List<CurvedLineRenderer>();

        public void Initialize()
        {
            Shown = true;
        }
        
    }
    
    public class FrameDrawingAbility : PlayerAbility
    {
        [Header("Drawing Settings")]
        [SerializeField] private CurvedLinePoint curvedPointPrefab;
        [SerializeField] private CurvedLineRenderer curvedLineContainerPrefab;
        
        [Space(7)]
        [SerializeField] private Material pen;
        [SerializeField] private Material transparentPen;
        
        // private Transform _frameParent; // The parent for lines drawing in a given frame
        private DrawingParent _currentDrawingParent; // the parent for the current frames' drawing
        private CurvedLineRenderer _lineParent; // The parent for an individual line
        private Hand _drawingHand;
        private CurvedLinePoint _drawingPoint;

        private List<DrawingParent> _drawingParents = new List<DrawingParent>(); // a list that holds drawings in a frame
        // private List<Transform> _frameContainers = new List<Transform>(); // a temp list that holds the drawing for each frame
        
        private bool _canDraw;
        private bool _drawingActionPressed;
        private bool _firstStroke;
        private float _widthPercentage;

        protected override void OnEnable()
        {
            base.OnEnable();

            FrameManager.SaveFrameCallback += SaveDrawing;
            FrameManager.LoadFrameCallback += LoadDrawing;
            FrameManager.EnterEditModeCallback += EnterEditMode;
            FrameManager.EnterPlayModeCallback += EnterPlayMode;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            FrameManager.SaveFrameCallback -= SaveDrawing;
            FrameManager.LoadFrameCallback -= LoadDrawing;
            FrameManager.EnterEditModeCallback -= EnterEditMode;
            FrameManager.EnterPlayModeCallback -= EnterPlayMode;
        }

        protected override void Start()
        {
            base.Start();

            _canDraw = false;
            
            CreateNewParent(0);
        }

        protected override void SetUpHandsOnStart()
        {
            base.SetUpHandsOnStart();
            
            switch (assignedPlayerComponent)
            {
                case AssignedPlayerComponent.DominantHand:
                    _drawingHand = PlayerManager.Instance.DominantHand;
                    break;
                case AssignedPlayerComponent.NonDominantHand:
                    _drawingHand = PlayerManager.Instance.NonDominantHand;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            _drawingPoint = Instantiate(curvedPointPrefab, _drawingHand.transform, true);
            _drawingPoint.GetComponent<MeshRenderer>().enabled = false;
            _drawingPoint.transform.localPosition = Vector3.zero;
        }

        protected override void PrimaryAction(InputEventArgs eventArgs)
        {
            base.PrimaryAction(eventArgs);

            switch (assignedPlayerComponent)
            {
                case AssignedPlayerComponent.BothHands:
                    _drawingHand = eventArgs.Hand;
                    break;
            }

            ToggleDrawingActionPressed(true);
        }

        protected override void SecondaryAction(InputEventArgs eventArgs)
        {
            _firstStroke = false;

            ToggleDrawingActionPressed(false);
        }

        protected override void TertiaryAction(InputEventArgs eventArgs)
        {
            ToggleDrawing(!_canDraw);
        }

        private void SaveDrawing(int frameToSave)
        {
            if (_drawingParents.Count == 0 || FrameManager.Instance.IsLastFrame())
            {
                AddDrawingsToParent(frameToSave);
                
                // CreateNewParent(frameToSave);

                HideAllDrawings();

                return;
            }
            
            // _drawingParents[frameToSave] = _drawingParents[frameToSave].transform;

            AddDrawingsToParent(frameToSave);

            HideAllDrawings();
        }

        private void LoadDrawing(int frameToLoad)
        {
            ShowDrawingFrame(frameToLoad);
        }

        private void EnterEditMode()
        {
            LoadDrawing(FrameManager.Instance.CurrentFrame);
        }

        private void EnterPlayMode()
        {
            HideAllDrawings();
        }

        private void ShowDrawingFrame(int frameToShow)
        {
            foreach (var parent in _drawingParents)
            {
                if (_drawingParents[frameToShow] == parent)
                {
                    parent.Shown = true;
                    foreach (var drawing in parent.LinesDrawn)
                    {
                        drawing.SetMaterial(pen);
                    }
                }
                else
                {
                    parent.Shown = false;
                    foreach (var drawing in parent.LinesDrawn)
                    {
                        drawing.SetMaterial(transparentPen);
                    }
                }
            }
        }

        private void HideAllDrawings()
        {
            foreach (var parent in _drawingParents)
            {
                if (!parent.Shown) continue;

                foreach (var drawing in parent.LinesDrawn)
                {
                    drawing.SetMaterial(transparentPen);
                }
                
                parent.Shown = false;
            }
            
        }

        private void CreateNewParent(int currentFrame)
        {
            var newParent = new DrawingParent();
            
            //var newParent = Instantiate(curvedLineContainerPrefab);
            // newParent.name = "Frame " + currentFrame + " Drawings";
            
            // _frameParents.Add(newParent);
            // newParent.Initialize();
            _currentDrawingParent = newParent;
        }

        private void AddDrawingsToParent(int frameToAddTo)
        {
            var frameDrawings = new DrawingParent();
            frameDrawings.Shown = true;
            
            foreach (var drawing in _currentDrawingParent.LinesDrawn)
            {
                frameDrawings.LinesDrawn.Add(drawing);
            }
            
            if (_drawingParents.Count == frameToAddTo)
            {
                _drawingParents.Add(frameDrawings);
            }
            else
            {
                _drawingParents[frameToAddTo] = frameDrawings;
            }
            
            _currentDrawingParent.LinesDrawn.Clear();

            // foreach (var drawing in _currentDrawingParent.linesDrawn)
            // {
            //     
            //     
            //     // _frameParents
            //     drawingContainer.parent = _frameParent;
            // }
            //
            // _frameContainers.Clear();
        }
        
        private void ToggleDrawing(bool toggle)
        {
            _canDraw = toggle;
            _drawingPoint.GetComponent<MeshRenderer>().enabled = toggle;
        }
        
        private void ToggleDrawingActionPressed(bool toggle)
        {
            _drawingActionPressed = toggle;
        }

        private void Update()
        {
            if (!_canDraw) return;
            
            if (!_drawingActionPressed) return;

            if (!_firstStroke)
            {
                // GameObject container = Instantiate(curvedLineContainerPrefab, FrameManager.Instance.GetCurrentFrame().transform);
                // var container = Instantiate(curvedLineContainerPrefab);
                // _lineParent.transform.parent = container.transform;
                // _lineParent.GetComponent<CurvedLineRenderer>().SetMaterial(pen);
            
                _currentDrawingParent.Initialize();

                _lineParent = Instantiate(curvedLineContainerPrefab);
                _lineParent.SetMaterial(pen);
                
                _currentDrawingParent.LinesDrawn.Add(_lineParent);


            }

            _firstStroke = true;
            
            Vector3 spawnPos = _drawingHand.transform.position;
            var cur = Instantiate(curvedPointPrefab, spawnPos, Quaternion.identity);
            //cur.transform.SetParent(FrameManager.Instance.GetCurrentFrame().transform);
            cur.transform.SetParent(_lineParent.transform);
        }
    }


}
