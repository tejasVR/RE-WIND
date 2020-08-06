using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace XREngine.Core.Scripts.VR.Player.Abilities
{
    public abstract class PlayerAbility : MonoBehaviour
    {
        protected enum AssignedPlayerComponent
        {
            DominantHand,
            NonDominantHand,
            BothHands,
            Head
        }

        [SerializeField] protected bool debug;

        [Header("Ability Settings")]
        [SerializeField] protected AssignedPlayerComponent assignedPlayerComponent;

        [Space(7)]
        [SerializeField] private PlayerInput.ButtonAction primaryButton;
        [SerializeField] private PlayerInput.ButtonAction secondaryButton;
        [SerializeField] private PlayerInput.ButtonAction tertiaryButton;
        
        [HideIf("debug"), PropertySpace]
        [Button(ButtonSizes.Medium), GUIColor(1, 1, 1)]
        private void TurnDebugOn()
        {
            debug = !debug;
        }

        [ShowIf("debug"), PropertySpace]
        [Button(ButtonSizes.Medium), GUIColor(0, 1, 1)]
        private void TurnOffDebug()
        {
            debug = !debug;
        }

        private Hand.HandOrientation _dominantHandOrientation;
        
        protected virtual void OnEnable()
        {
            SubscribeActions();

            PlayerManager.SwitchDominantHandCallback += SwitchedDominantHands;
        }

        protected  virtual void OnDisable()
        {
            UnsubscribeActions();

            PlayerManager.SwitchDominantHandCallback -= SwitchedDominantHands;
        }

        protected virtual void Start()
        {
            SetUpHandsOnStart();
        }

        protected virtual void SetUpHandsOnStart() { }

        protected virtual void OnSwitchHands() { }
        
        protected virtual void PrimaryAction(InputEventArgs eventArgs) { }

        protected virtual void SecondaryAction(InputEventArgs eventArgs) { }

        protected virtual void TertiaryAction(InputEventArgs eventArgs) { }

        private void SwitchedDominantHands()
        {
            UnsubscribeActions();
            SubscribeActions();
            OnSwitchHands();
        }

        private void SubscribeActions()
        {
            if (PlayerManager.Instance == null)
            {
                Debug.Log("no PlayerManager");
            }
            
            var dominantHand = PlayerManager.Instance.DominantHand.Handedness;

            switch (assignedPlayerComponent)
            {
                case AssignedPlayerComponent.BothHands:
                    SubscribeLeftHand();
                    SubscribeRightHand();
                    break;
                case AssignedPlayerComponent.DominantHand when dominantHand == Hand.HandOrientation.Left:
                    SubscribeLeftHand();
                    break;
                case AssignedPlayerComponent.DominantHand when dominantHand == Hand.HandOrientation.Right:
                    SubscribeRightHand();
                    break;
                case AssignedPlayerComponent.NonDominantHand when dominantHand == Hand.HandOrientation.Left:
                    SubscribeRightHand();
                    break;
                case AssignedPlayerComponent.NonDominantHand when dominantHand == Hand.HandOrientation.Right:
                    SubscribeLeftHand();
                    break;
            }
        }

        private void UnsubscribeActions()
        {
            UnsubscribeLeftHand();
            UnsubscribeRightHand();
        }
        
        private void SubscribeLeftHand()
        {
            SubscribeLeftEvents(primaryButton, PrimaryAction);
            SubscribeLeftEvents(secondaryButton, SecondaryAction);
            SubscribeLeftEvents(tertiaryButton, TertiaryAction);
        }

        private void UnsubscribeLeftHand()
        {
            UnsubscribeLeftEvents(primaryButton, PrimaryAction);
            UnsubscribeLeftEvents(secondaryButton, SecondaryAction);
            UnsubscribeLeftEvents(tertiaryButton, TertiaryAction);
        }

        private void SubscribeRightHand()
        {
            SubscribeRightEvents(primaryButton, PrimaryAction);
            SubscribeRightEvents(secondaryButton, SecondaryAction);
            SubscribeRightEvents(tertiaryButton, TertiaryAction);
        }

        private void UnsubscribeRightHand()
        {
            UnsubscribeRightEvents(primaryButton, PrimaryAction);
            UnsubscribeRightEvents(secondaryButton, SecondaryAction);
            UnsubscribeRightEvents(tertiaryButton, TertiaryAction);
        }

        private static void SubscribeLeftEvents(PlayerInput.ButtonAction buttonAction, PlayerInput.InputEventHandler actionMethod)
        {
             switch (buttonAction)
            {
                case PlayerInput.ButtonAction.None:
                    break;
                
                // Trigger Functions
                case PlayerInput.ButtonAction.TriggerDown:
                    PlayerInput.LTriggerDownCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.TriggerPressed:
                    PlayerInput.LTriggerPressedCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.TriggerUp:
                    PlayerInput.LTriggerUpCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.TriggerValueChanged:
                    PlayerInput.LTriggerValueChangedCallback += actionMethod;
                    break;
                
                // Grip Functions
                case PlayerInput.ButtonAction.GripDown:
                    PlayerInput.LGripDownCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.GripPressed:
                    PlayerInput.LGripPressedCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.GripUp:
                    PlayerInput.LGripUpCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.GripValueChanged:
                    PlayerInput.LGripValueChangedCallback += actionMethod;
                    break;
                
                // Auxiliary Button Functions
                
                // --- Button One
                case PlayerInput.ButtonAction.ButtonOneTouchDown:
                    PlayerInput.LButtonOneTouchDownCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.ButtonOneTouchPressed:
                    PlayerInput.LButtonOneTouchPressedCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.ButtonOneTouchUp:
                    PlayerInput.LButtonOneTouchUpCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.ButtonOneClickDown:
                    PlayerInput.LButtonOneClickDownCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.ButtonOneClickPressed:
                    PlayerInput.LButtonOneClickPressedCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.ButtonOneClickUp:
                    PlayerInput.LButtonOneClickUpCallback += actionMethod;
                    break;
                
                // --- Button Two
                case PlayerInput.ButtonAction.ButtonTwoTouchDown:
                    PlayerInput.LButtonTwoTouchDownCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.ButtonTwoTouchPressed:
                    PlayerInput.LButtonTwoTouchPressedCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.ButtonTwoTouchUp:
                    PlayerInput.LButtonTwoTouchUpCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.ButtonTwoClickDown:
                    PlayerInput.LButtonTwoClickDownCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.ButtonTwoClickPressed:
                    PlayerInput.LButtonTwoClickPressedCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.ButtonTwoClickUp:
                    PlayerInput.LButtonTwoClickUpCallback += actionMethod;
                    break;
                
                // Joystick Functions
                
                // --- Joystick Touch
                case PlayerInput.ButtonAction.JoystickTouchDown:
                    PlayerInput.LJoystickTouchDownCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickTouchPressed:
                    PlayerInput.LJoystickTouchPressedCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickTouchUp:
                    PlayerInput.LJoystickTouchUpCallback += actionMethod;
                    break;
                
              // --- Joystick Click
                case PlayerInput.ButtonAction.JoystickClickDown:
                    PlayerInput.LJoystickClickDownCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickClickPressed:
                    PlayerInput.LJoystickClickPressedCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickClickUp:
                    PlayerInput.LJoystickClickUpCallback += actionMethod;
                    break;
                
                // -- Joystick Tilt
                case PlayerInput.ButtonAction.JoystickLeft:
                    PlayerInput.LJoystickTiltLeftCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickLeftPressed:
                    PlayerInput.LJoystickTiltLeftPressedCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickRight:
                    PlayerInput.LJoystickTiltRightCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickRightPressed:
                    PlayerInput.LJoystickTiltRightPressedCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickUp:
                    PlayerInput.LJoystickTiltUpCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickUpPressed:
                    PlayerInput.LJoystickTiltUpPressedCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickDown:
                    PlayerInput.LJoystickTiltDownCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickDownPressed:
                    PlayerInput.LJoystickTiltDownPressedCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickValueChanged:
                    PlayerInput.LJoystickValueChangedCallback += actionMethod;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        private static void UnsubscribeLeftEvents(PlayerInput.ButtonAction buttonAction, PlayerInput.InputEventHandler actionMethod)
        {
             switch (buttonAction)
            {
                case PlayerInput.ButtonAction.None:
                    break;
                
                // Trigger Functions
                case PlayerInput.ButtonAction.TriggerDown:
                    PlayerInput.LTriggerDownCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.TriggerPressed:
                    PlayerInput.LTriggerPressedCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.TriggerUp:
                    PlayerInput.LTriggerUpCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.TriggerValueChanged:
                    PlayerInput.LTriggerValueChangedCallback -= actionMethod;
                    break;
                
                // Grip Functions
                case PlayerInput.ButtonAction.GripDown:
                    PlayerInput.LGripDownCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.GripPressed:
                    PlayerInput.LGripPressedCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.GripUp:
                    PlayerInput.LGripUpCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.GripValueChanged:
                    PlayerInput.LGripValueChangedCallback -= actionMethod;
                    break;
                
                // Auxiliary Button Functions
                
                // --- Button One
                case PlayerInput.ButtonAction.ButtonOneTouchDown:
                    PlayerInput.LButtonOneTouchDownCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.ButtonOneTouchPressed:
                    PlayerInput.LButtonOneTouchPressedCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.ButtonOneTouchUp:
                    PlayerInput.LButtonOneTouchUpCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.ButtonOneClickDown:
                    PlayerInput.LButtonOneClickDownCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.ButtonOneClickPressed:
                    PlayerInput.LButtonOneClickPressedCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.ButtonOneClickUp:
                    PlayerInput.LButtonOneClickUpCallback -= actionMethod;
                    break;
                
                // --- Button Two
                case PlayerInput.ButtonAction.ButtonTwoTouchDown:
                    PlayerInput.LButtonTwoTouchDownCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.ButtonTwoTouchPressed:
                    PlayerInput.LButtonTwoTouchPressedCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.ButtonTwoTouchUp:
                    PlayerInput.LButtonTwoTouchUpCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.ButtonTwoClickDown:
                    PlayerInput.LButtonTwoClickDownCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.ButtonTwoClickPressed:
                    PlayerInput.LButtonTwoClickPressedCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.ButtonTwoClickUp:
                    PlayerInput.LButtonTwoClickUpCallback -= actionMethod;
                    break;
                
                // Joystick Functions
                
                // --- Joystick Touch
                case PlayerInput.ButtonAction.JoystickTouchDown:
                    PlayerInput.LJoystickTouchDownCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickTouchPressed:
                    PlayerInput.LJoystickTouchPressedCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickTouchUp:
                    PlayerInput.LJoystickTouchUpCallback -= actionMethod;
                    break;
                
              // --- Joystick Click
                case PlayerInput.ButtonAction.JoystickClickDown:
                    PlayerInput.LJoystickClickDownCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickClickPressed:
                    PlayerInput.LJoystickClickPressedCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickClickUp:
                    PlayerInput.LJoystickClickUpCallback -= actionMethod;
                    break;
                
                // -- Joystick Tilt
                case PlayerInput.ButtonAction.JoystickLeft:
                    PlayerInput.LJoystickTiltLeftCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickLeftPressed:
                    PlayerInput.LJoystickTiltLeftPressedCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickRight:
                    PlayerInput.LJoystickTiltRightCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickRightPressed:
                    PlayerInput.LJoystickTiltRightPressedCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickUp:
                    PlayerInput.LJoystickTiltUpCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickUpPressed:
                    PlayerInput.LJoystickTiltUpPressedCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickDown:
                    PlayerInput.LJoystickTiltDownCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickDownPressed:
                    PlayerInput.LJoystickTiltDownPressedCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickValueChanged:
                    PlayerInput.LJoystickValueChangedCallback -= actionMethod;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void SubscribeRightEvents(PlayerInput.ButtonAction buttonAction, PlayerInput.InputEventHandler actionMethod)
        {
             switch (buttonAction)
            {
                case PlayerInput.ButtonAction.None:
                    break;
                
                // Trigger Functions
                case PlayerInput.ButtonAction.TriggerDown:
                    PlayerInput.RTriggerDownCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.TriggerPressed:
                    PlayerInput.RTriggerPressedCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.TriggerUp:
                    PlayerInput.RTriggerUpCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.TriggerValueChanged:
                    PlayerInput.RTriggerValueChangedCallback += actionMethod;
                    break;
                
                // Grip Functions
                case PlayerInput.ButtonAction.GripDown:
                    PlayerInput.RGripDownCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.GripPressed:
                    PlayerInput.RGripPressedCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.GripUp:
                    PlayerInput.RGripUpCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.GripValueChanged:
                    PlayerInput.RGripValueChangedCallback += actionMethod;
                    break;
                
                // Auxiliary Button Functions
                
                // --- Button One
                case PlayerInput.ButtonAction.ButtonOneTouchDown:
                    PlayerInput.RButtonOneTouchDownCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.ButtonOneTouchPressed:
                    PlayerInput.RButtonOneTouchPressedCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.ButtonOneTouchUp:
                    PlayerInput.RButtonOneTouchUpCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.ButtonOneClickDown:
                    PlayerInput.RButtonOneClickDownCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.ButtonOneClickPressed:
                    PlayerInput.RButtonOneClickPressedCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.ButtonOneClickUp:
                    PlayerInput.RButtonOneClickUpCallback += actionMethod;
                    break;
                
                // --- Button Two
                case PlayerInput.ButtonAction.ButtonTwoTouchDown:
                    PlayerInput.RButtonTwoTouchDownCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.ButtonTwoTouchPressed:
                    PlayerInput.RButtonTwoTouchPressedCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.ButtonTwoTouchUp:
                    PlayerInput.RButtonTwoTouchUpCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.ButtonTwoClickDown:
                    PlayerInput.RButtonTwoClickDownCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.ButtonTwoClickPressed:
                    PlayerInput.RButtonTwoClickPressedCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.ButtonTwoClickUp:
                    PlayerInput.RButtonTwoClickUpCallback += actionMethod;
                    break;
                
                // Joystick Functions
                
                // --- Joystick Touch
                case PlayerInput.ButtonAction.JoystickTouchDown:
                    PlayerInput.RJoystickTouchDownCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickTouchPressed:
                    PlayerInput.RJoystickTouchPressedCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickTouchUp:
                    PlayerInput.RJoystickTouchUpCallback += actionMethod;
                    break;
                
              // --- Joystick Click
                case PlayerInput.ButtonAction.JoystickClickDown:
                    PlayerInput.RJoystickClickDownCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickClickPressed:
                    PlayerInput.RJoystickClickPressedCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickClickUp:
                    PlayerInput.RJoystickClickUpCallback += actionMethod;
                    break;
                
                // -- Joystick Tilt
                case PlayerInput.ButtonAction.JoystickLeft:
                    PlayerInput.RJoystickTiltLeftCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickLeftPressed:
                    PlayerInput.RJoystickTiltLeftPressedCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickRight:
                    PlayerInput.RJoystickTiltRightCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickRightPressed:
                    PlayerInput.RJoystickTiltRightPressedCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickUp:
                    PlayerInput.RJoystickTiltUpCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickUpPressed:
                    PlayerInput.RJoystickTiltUpPressedCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickDown:
                    PlayerInput.RJoystickTiltDownCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickDownPressed:
                    PlayerInput.RJoystickTiltDownPressedCallback += actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickValueChanged:
                    PlayerInput.RJoystickValueChangedCallback += actionMethod;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        private static void UnsubscribeRightEvents(PlayerInput.ButtonAction buttonAction, PlayerInput.InputEventHandler actionMethod)
        {
             switch (buttonAction)
            {
                case PlayerInput.ButtonAction.None:
                    break;
                
                // Trigger Functions
                case PlayerInput.ButtonAction.TriggerDown:
                    PlayerInput.RTriggerDownCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.TriggerPressed:
                    PlayerInput.RTriggerPressedCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.TriggerUp:
                    PlayerInput.RTriggerUpCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.TriggerValueChanged:
                    PlayerInput.RTriggerValueChangedCallback -= actionMethod;
                    break;
                
                // Grip Functions
                case PlayerInput.ButtonAction.GripDown:
                    PlayerInput.RGripDownCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.GripPressed:
                    PlayerInput.RGripPressedCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.GripUp:
                    PlayerInput.RGripUpCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.GripValueChanged:
                    PlayerInput.RGripValueChangedCallback -= actionMethod;
                    break;
                
                // Auxiliary Button Functions
                
                // --- Button One
                case PlayerInput.ButtonAction.ButtonOneTouchDown:
                    PlayerInput.RButtonOneTouchDownCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.ButtonOneTouchPressed:
                    PlayerInput.RButtonOneTouchPressedCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.ButtonOneTouchUp:
                    PlayerInput.RButtonOneTouchUpCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.ButtonOneClickDown:
                    PlayerInput.RButtonOneClickDownCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.ButtonOneClickPressed:
                    PlayerInput.RButtonOneClickPressedCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.ButtonOneClickUp:
                    PlayerInput.RButtonOneClickUpCallback -= actionMethod;
                    break;
                
                // --- Button Two
                case PlayerInput.ButtonAction.ButtonTwoTouchDown:
                    PlayerInput.RButtonTwoTouchDownCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.ButtonTwoTouchPressed:
                    PlayerInput.RButtonTwoTouchPressedCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.ButtonTwoTouchUp:
                    PlayerInput.RButtonTwoTouchUpCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.ButtonTwoClickDown:
                    PlayerInput.RButtonTwoClickDownCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.ButtonTwoClickPressed:
                    PlayerInput.RButtonTwoClickPressedCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.ButtonTwoClickUp:
                    PlayerInput.RButtonTwoClickUpCallback -= actionMethod;
                    break;
                
                // Joystick Functions
                
                // --- Joystick Touch
                case PlayerInput.ButtonAction.JoystickTouchDown:
                    PlayerInput.RJoystickTouchDownCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickTouchPressed:
                    PlayerInput.RJoystickTouchPressedCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickTouchUp:
                    PlayerInput.RJoystickTouchUpCallback -= actionMethod;
                    break;
                
              // --- Joystick Click
                case PlayerInput.ButtonAction.JoystickClickDown:
                    PlayerInput.RJoystickClickDownCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickClickPressed:
                    PlayerInput.RJoystickClickPressedCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickClickUp:
                    PlayerInput.RJoystickClickUpCallback -= actionMethod;
                    break;
                
                // -- Joystick Tilt
                case PlayerInput.ButtonAction.JoystickLeft:
                    PlayerInput.RJoystickTiltLeftCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickLeftPressed:
                    PlayerInput.RJoystickTiltLeftPressedCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickRight:
                    PlayerInput.RJoystickTiltRightCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickRightPressed:
                    PlayerInput.RJoystickTiltRightPressedCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickUp:
                    PlayerInput.RJoystickTiltUpCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickUpPressed:
                    PlayerInput.RJoystickTiltUpPressedCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickDown:
                    PlayerInput.RJoystickTiltDownCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickDownPressed:
                    PlayerInput.RJoystickTiltDownPressedCallback -= actionMethod;
                    break;
                case PlayerInput.ButtonAction.JoystickValueChanged:
                    PlayerInput.RJoystickValueChangedCallback -= actionMethod;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
