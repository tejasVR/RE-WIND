using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using Hand = XREngine.Core.Scripts.VR.Player.Hand;

namespace XREngine.Core.Scripts.VR.Player
{
    /// <summary>
    /// An class to contain a generic type of input from controllers or external sensors (i.e., hand tracking)
    /// </summary>
    [Serializable] 
    public class Input
    {
        public Input(InputDevice device, Hand hand, InputFeatureUsage<bool> boolButton)
        {
            this.device = device;
            this.hand = hand;
            boolInputUsage = boolButton;
        }        

        public Input(InputDevice device, Hand hand, InputFeatureUsage<float> vector1Button, InputFeatureUsage<bool> boolButton)
        {
            this.device = device;
            this.hand = hand;
            vector1InputUsage = vector1Button;
            boolInputUsage = boolButton;
        }

        public Input(InputDevice device, Hand hand, InputFeatureUsage<Vector2> vector2Button)
        {
            this.device = device;
            this.hand = hand;
            vector2InputUsage = vector2Button;
        }
        
        public InputDevice device;
        public Hand hand;
        public InputFeatureUsage<bool> boolInputUsage;
        public InputFeatureUsage<float> vector1InputUsage;
        public InputFeatureUsage<Vector2> vector2InputUsage;

        public bool boolValue;
        public float vector1Value;
        public Vector2 vector2Value;
        public bool leftTilt;
        public bool rightTilt;
        public bool upTilt;
        public bool downTilt;
    }

    [Serializable]
    public struct InputEventArgs
    {
        public InputEventArgs(Hand hand)
        {
            Hand = hand;
        }
        public Hand Hand { get; }
    }

    /// <summary>
    /// A script that houses all Player input 
    /// </summary>
    public class PlayerInput : MonoBehaviour
    {
        #region EVENT CREATION
        
        public delegate void InputEventHandler(InputEventArgs e);
        
        // public static ButtonEvent
        public static ButtonEventTest TestButtonEvent;
        public static event Action<InputEventArgs> TestInputEvent;                              // Called the frame if the right trigger is pressed down
        
        public static event InputEventHandler RTriggerDownCallback;                              // Called the frame if the right trigger is pressed down
        public static event InputEventHandler RTriggerPressedCallback;                           // Called any frame if the right trigger is pressed
        public static event InputEventHandler RTriggerUpCallback;                                // Called the frame if the right trigger is released
        public static event InputEventHandler RTriggerValueChangedCallback;                             // Called the frame if the right trigger is changed in value

        public static event InputEventHandler RGripDownCallback;                                 // Called the frame if the right grip is pressed down
        public static event InputEventHandler RGripPressedCallback;                              // Called any frame if the right grip is pressed
        public static event InputEventHandler RGripUpCallback;                                   // Called the frame if the right grip is released
        public static event InputEventHandler RGripValueChangedCallback;

        public static event InputEventHandler RJoystickTouchDownCallback;                        // Called the frame if the right Joystick is pressed down
        public static event InputEventHandler RJoystickTouchPressedCallback;                     // Called the frame if the right Joystick is pressed
        public static event InputEventHandler RJoystickTouchUpCallback;                          // Called the frame if the right Joystick is released
        
        public static event InputEventHandler RJoystickClickDownCallback;                        // Called the frame if the right Joystick is pressed down
        public static event InputEventHandler RJoystickClickPressedCallback;                     // Called the frame if the right Joystick is pressed
        public static event InputEventHandler RJoystickClickUpCallback;                          // Called the frame if the right Joystick is released
        
        public static event InputEventHandler RJoystickTiltLeftCallback;                         // Called the frame if the right Joystick axis is moved
        public static event InputEventHandler RJoystickTiltLeftPressedCallback;                         // Called the frame if the right Joystick axis is moved
        public static event InputEventHandler RJoystickTiltRightCallback;                        // Called the frame if the right Joystick axis is moved
        public static event InputEventHandler RJoystickTiltRightPressedCallback;                        // Called the frame if the right Joystick axis is moved
        public static event InputEventHandler RJoystickTiltUpCallback;                           // Called the frame if the right Joystick axis is moved
        public static event InputEventHandler RJoystickTiltUpPressedCallback;                           // Called the frame if the right Joystick axis is moved
        public static event InputEventHandler RJoystickTiltDownCallback;                         // Called the frame if the right Joystick axis is moved
        public static event InputEventHandler RJoystickTiltDownPressedCallback;                         // Called the frame if the right Joystick axis is moved
        public static event InputEventHandler RJoystickValueChangedCallback;                            // Called the frame if the right Joystick axis is moved
        
        public static event InputEventHandler LTriggerDownCallback;                              // Called the frame if the left trigger is pressed down
        public static event InputEventHandler LTriggerPressedCallback;                           // Called any frame if the left trigger is pressed
        public static event InputEventHandler LTriggerUpCallback;                                // Called the frame if the left trigger is released
        public static event InputEventHandler LTriggerValueChangedCallback;                      // Called the frame if the right trigger is changed in value

        public static event InputEventHandler LGripDownCallback;                                 // Called the frame if the left grip is pressed down
        public static event InputEventHandler LGripPressedCallback;                              // Called any frame if the left grip is pressed
        public static event InputEventHandler LGripUpCallback;                                   // Called the frame if the left grip is released
        public static event InputEventHandler LGripValueChangedCallback;

        public static event InputEventHandler LJoystickTouchDownCallback;                        // Called the frame if the right Joystick is pressed down
        public static event InputEventHandler LJoystickTouchPressedCallback;                     // Called the frame if the right Joystick is pressed
        public static event InputEventHandler LJoystickTouchUpCallback;                          // Called the frame if the right Joystick is released
        
        public static event InputEventHandler LJoystickClickDownCallback;                        // Called the frame if the left Joystick is pressed down
        public static event InputEventHandler LJoystickClickPressedCallback;                     // Called the frame if the left Joystick is pressed
        public static event InputEventHandler LJoystickClickUpCallback;                          // Called the frame if the left Joystick is released
        
        public static event InputEventHandler LJoystickTiltLeftCallback;                         // Called the frame if the right Joystick axis is moved
        public static event InputEventHandler LJoystickTiltLeftPressedCallback;
        public static event InputEventHandler LJoystickTiltRightCallback;                        // Called the frame if the right Joystick axis is moved
        public static event InputEventHandler LJoystickTiltRightPressedCallback;
        public static event InputEventHandler LJoystickTiltUpCallback;                           // Called the frame if the right Joystick axis is moved
        public static event InputEventHandler LJoystickTiltUpPressedCallback;
        public static event InputEventHandler LJoystickTiltDownCallback;                         // Called the frame if the right Joystick axis is moved
        public static event InputEventHandler LJoystickTiltDownPressedCallback;
        public static event InputEventHandler LJoystickValueChangedCallback;                            // Called the frame if the left Joystick axis is moved
        
        public static event InputEventHandler LButtonOneTouchDownCallback;                            // Called the frame if the A Button is pressed down
        public static event InputEventHandler LButtonOneTouchPressedCallback;                         // Called any frame if the A Button is pressed
        public static event InputEventHandler LButtonOneTouchUpCallback;                              // Called the frame if the A Button is released
        
        public static event InputEventHandler LButtonOneClickDownCallback;                            // Called the frame if the A Button is pressed down
        public static event InputEventHandler LButtonOneClickPressedCallback;                         // Called any frame if the A Button is pressed
        public static event InputEventHandler LButtonOneClickUpCallback;                              // Called the frame if the A Button is released

        public static event InputEventHandler LButtonTwoTouchDownCallback;                            // Called the frame if the A Button is pressed down
        public static event InputEventHandler LButtonTwoTouchPressedCallback;                         // Called any frame if the A Button is pressed
        public static event InputEventHandler LButtonTwoTouchUpCallback;                              // Called the frame if the A Button is released
        
        public static event InputEventHandler LButtonTwoClickDownCallback;                            // Called the frame if the B Button is pressed down
        public static event InputEventHandler LButtonTwoClickPressedCallback;                         // Called any frame if the b Button is pressed
        public static event InputEventHandler LButtonTwoClickUpCallback;                              // Called the frame if the B Button is released

        public static event InputEventHandler RButtonOneTouchDownCallback;                            // Called the frame if the A Button is pressed down
        public static event InputEventHandler RButtonOneTouchPressedCallback;                         // Called any frame if the A Button is pressed
        public static event InputEventHandler RButtonOneTouchUpCallback;                              // Called the frame if the A Button is released
        
        public static event InputEventHandler RButtonOneClickDownCallback;                            // Called the frame if the A Button is pressed down
        public static event InputEventHandler RButtonOneClickPressedCallback;                         // Called any frame if the A Button is pressed
        public static event InputEventHandler RButtonOneClickUpCallback;                              // Called the frame if the A Button is released

        public static event InputEventHandler RButtonTwoTouchDownCallback;                            // Called the frame if the A Button is pressed down
        public static event InputEventHandler RButtonTwoTouchPressedCallback;                         // Called any frame if the A Button is pressed
        public static event InputEventHandler RButtonTwoTouchUpCallback;                              // Called the frame if the A Button is released
        
        public static event InputEventHandler RButtonTwoClickDownCallback;                            // Called the frame if the B Button is pressed down
        public static event InputEventHandler RButtonTwoClickPressedCallback;                         // Called any frame if the b Button is pressed
        public static event InputEventHandler RButtonTwoClickUpCallback;                              // Called the frame if the B Button is released

        public static event InputEventHandler MenuButtonDownCallback;                                  // Called any frame if the menu button is pressed down
        public static event InputEventHandler MenuButtonPressedCallback;                               // Called any frame if the menu button is pressed down
        public static event InputEventHandler MenuButtonUpCallback;                                    // Called any frame if the menu button is released

        #endregion EVENT CREATION

        #region VARIABLES
        
        [SerializeField] private bool debug;

        [Space(7)]
        [SerializeField] private Hand leftHand;
        [SerializeField] private Hand rightHand;

        // [Space(7)]
        [Header("Trigger Properties")]
        [Tooltip("The amount of which the trigger needs to be pressed down to be recognized as a down event")]
        [SerializeField] [Range(0, 1)]
        private float triggerDownThreshold;
        [Tooltip("The amount of which the trigger needs to be pressed up to be recognized as an up event")]
        [SerializeField] [Range(0, 1)]
        private float triggerUpThreshold;

        [Header("Grip Properties")]
        [Tooltip("The amount of which the grip needs to be pressed down to be recognized as a down event")]
        [SerializeField] [Range(0, 1)]
        private float gripDownThreshold;
        [Tooltip("The amount of which the grip needs to be pressed up to be recognized as an up event")]
        [SerializeField] [Range(0, 1)]
        private float gripUpThreshold;
        
        [Space(7)]
        [Header("Joystick Axis Properties")]
        [Tooltip("The amount of which the axis needs to be titled to the left to be recognized as a left event")]
        [SerializeField] [Range(-1, 0)]
        private float leftTiltThreshold;
        [Tooltip("The amount of which the axis needs to be tilted to the right to be recognized as a right event")]
        [SerializeField] [Range(0, 1)]
        private float rightTiltThreshold;
        [Tooltip("The amount of which the axis needs to be tilted to the up to be recognized as an up event")]
        [SerializeField] [Range(0, 1)]
        private float upTiltThreshold;
        [Tooltip("The amount of which the axis needs to be tilted to the down to be recognized as a down event")]
        [SerializeField] [Range(-1, 0)]
        private float downTiltThreshold;

        public enum ButtonAction
        {
            None,
            TriggerDown,
            TriggerPressed,
            TriggerUp,
            TriggerValueChanged,
            GripDown,
            GripPressed,
            GripUp,
            GripValueChanged,
            ButtonOneTouchDown,
            ButtonOneTouchPressed,
            ButtonOneTouchUp,
            ButtonOneClickDown,
            ButtonOneClickPressed,
            ButtonOneClickUp,
            ButtonTwoTouchDown,
            ButtonTwoTouchPressed,
            ButtonTwoTouchUp,
            ButtonTwoClickDown,
            ButtonTwoClickPressed,
            ButtonTwoClickUp,
            JoystickTouchDown,
            JoystickTouchPressed,
            JoystickTouchUp,
            JoystickClickDown,
            JoystickClickPressed,
            JoystickClickUp,
            JoystickLeft,
            JoystickLeftPressed,
            JoystickRight,
            JoystickRightPressed,
            JoystickUp,
            JoystickUpPressed,
            JoystickDown,
            JoystickDownPressed,
            JoystickValueChanged
        }

        // Left Hand
        private Input _leftTrigger;
        private Input _leftGrip;

        private Input _leftJoystickTouch;
        private Input _leftJoystickClick;
        private Input _leftJoystickTilt;

        private Input _leftButtonOneTouch;
        private Input _leftButtonOneClick;
        private Input _leftButtonTwoTouch;
        private Input _leftButtonTwoClick;
        
        //Right Hand
        private Input _rightTrigger;
        private Input _rightGrip;

        private Input _rightJoystickTouch;
        private Input _rightJoystickClick;
        private Input _rightJoystickTilt;

        private Input _rightButtonOneTouch;
        private Input _rightButtonOneClick;
        private Input _rightButtonTwoTouch;
        private Input _rightButtonTwoClick;

        // Universal
        private Input _leftMenuButton;
        private Input _rightMenuButton;

        private bool _controllersFound;

        // private Dictionary<ButtonEvent, ButtonAction> RightButtonActions =
        //     new Dictionary<ButtonEvent, ButtonAction>()
        //     {
        //         {TestButtonEvent, ButtonAction.TriggerDown },
        //     };
            /*{
                // Trigger Functions
                {ButtonAction.TriggerDown, RTriggerDownCallback},
                {ButtonAction.TriggerPressed, RTriggerPressedCallback},
                {ButtonAction.TriggerUp, RTriggerUpCallback},
                {ButtonAction.TriggerValueChanged, RTriggerValueChangedCallback},
            };*/
        
        private InputEventHandler[] _inputArray = new InputEventHandler[]{RTriggerDownCallback};
        
        private InputDevice _rightDevice;
        private InputDevice _leftDevice;
        
        private bool _leftHandIndexPinching;
        private bool _rightHandIndexPinching;
        
        #endregion VARIABLES
        
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

        private void Start()
        {
            TestButtonEvent = new ButtonEventTest(LTriggerDownCallback);
            
            // _inputList.Add(RTriggerDownCallback.);
            AssignInputDevices();
            
            InitializeButtonInput();
            AssembleDictionaries();
        }

        private void InitializeButtonInput()
        {
            if (debug)
                Debug.Log("Initializing button input");

            // Left Hand
            _leftTrigger = new Input(_leftDevice, leftHand, CommonUsages.trigger, CommonUsages.triggerButton);
            _leftGrip = new Input(_leftDevice, leftHand, CommonUsages.grip, CommonUsages.gripButton);
            
            _leftJoystickTouch = new Input(_leftDevice, leftHand, CommonUsages.primary2DAxisTouch);
            _leftJoystickClick = new Input(_leftDevice, leftHand, CommonUsages.primary2DAxisClick);
            _leftJoystickTilt = new Input(_leftDevice, leftHand, CommonUsages.primary2DAxis);
            
            _leftMenuButton = new Input(_leftDevice, leftHand, CommonUsages.menuButton);
            
            _leftButtonOneTouch = new Input(_leftDevice, leftHand, CommonUsages.primaryTouch);
            _leftButtonOneClick = new Input(_leftDevice, leftHand, CommonUsages.primaryButton);
            
            _leftButtonTwoTouch = new Input(_leftDevice, leftHand, CommonUsages.secondaryTouch);
            _leftButtonTwoClick = new Input(_leftDevice, leftHand, CommonUsages.secondaryButton);

            // Right Hand
            _rightTrigger = new Input(_rightDevice, rightHand, CommonUsages.trigger, CommonUsages.triggerButton);
            _rightGrip = new Input(_rightDevice, rightHand, CommonUsages.grip, CommonUsages.gripButton);
            
            _rightJoystickTouch = new Input(_rightDevice, rightHand, CommonUsages.primary2DAxisTouch);
            _rightJoystickClick = new Input(_rightDevice, rightHand, CommonUsages.primary2DAxisClick);
            _rightJoystickTilt = new Input(_rightDevice, rightHand, CommonUsages.primary2DAxis);
            
            _rightButtonOneTouch = new Input(_rightDevice, rightHand, CommonUsages.primaryTouch);
            _rightButtonOneClick = new Input(_rightDevice, rightHand, CommonUsages.primaryButton);

            _rightButtonTwoTouch = new Input(_rightDevice, rightHand, CommonUsages.primaryTouch);
            _rightButtonTwoClick = new Input(_rightDevice, rightHand, CommonUsages.secondaryButton);
        }

        private void AssembleDictionaries()
        {
            //RightButtonActions.Add(TestButtonEvent, ButtonAction.TriggerDown);
            // RightButtonActions.Add(ButtonAction.TriggerDown, RTriggerDownCallback);
            // RightButtonActions.Add(ButtonAction.TriggerPressed, RTriggerPressedCallback);
            
            /*RightButtonActions = new Dictionary<ButtonAction, Action<Hand>>()
            {
                // Trigger Functions
                {ButtonAction.TriggerDown, RTriggerDownCallback},
                {ButtonAction.TriggerPressed, RTriggerPressedCallback},
                {ButtonAction.TriggerUp, RTriggerUpCallback},
                {ButtonAction.TriggerValueChanged, RTriggerValueChangedCallback},
            };*/

            // var count = RightButtonActions.Count;

        }

        private void Update()
        {
            OculusControllerInput();

        }

        
        private void OculusHandTrackingInput()
        {
            if (_rightDevice.TryGetFeatureValue(CommonUsages.handData, out var handValue))
            {
                Debug.Log(handValue);
            }
        }

        private void OculusControllerInput()
        {
            // Trigger Buttons
            UpdateVector1Button(_leftTrigger, _leftTrigger.hand, triggerDownThreshold,triggerUpThreshold, 
                LTriggerDownCallback, LTriggerPressedCallback, LTriggerUpCallback, LTriggerValueChangedCallback);
            
            UpdateVector1Button(_rightTrigger, _rightTrigger.hand, triggerDownThreshold,triggerUpThreshold,
                RTriggerDownCallback, RTriggerPressedCallback, RTriggerUpCallback, RTriggerValueChangedCallback);

            
            // Grip Buttons
            UpdateVector1Button(_leftGrip, _leftGrip.hand, gripDownThreshold, gripUpThreshold, 
                LGripDownCallback, LGripPressedCallback, LGripUpCallback, LGripValueChangedCallback);
            
            UpdateVector1Button(_rightGrip, _rightGrip.hand, gripDownThreshold, gripUpThreshold, 
                RGripDownCallback, RGripPressedCallback, RGripUpCallback, RGripValueChangedCallback);
            
            
            // Auxiliary Buttons
            
            // --- Left
            UpdateBoolButton(_leftButtonOneTouch, _leftButtonOneTouch.hand, LButtonOneTouchDownCallback, LButtonOneTouchPressedCallback, LButtonOneTouchUpCallback);
            UpdateBoolButton(_leftButtonOneClick, _leftButtonOneClick.hand, LButtonOneClickDownCallback, LButtonOneClickPressedCallback, LButtonOneClickUpCallback);
            
            UpdateBoolButton(_leftButtonTwoTouch, _leftButtonTwoTouch.hand, LButtonTwoTouchDownCallback, LButtonTwoTouchPressedCallback, LButtonTwoTouchUpCallback);
            UpdateBoolButton(_leftButtonTwoClick, _leftButtonTwoClick.hand, LButtonTwoClickDownCallback, LButtonTwoClickPressedCallback, LButtonTwoClickUpCallback);
            
            // --- Right
            UpdateBoolButton(_rightButtonOneTouch, _rightButtonOneTouch.hand, RButtonOneTouchDownCallback, RButtonOneTouchPressedCallback, RButtonOneTouchUpCallback);
            UpdateBoolButton(_rightButtonOneClick, _rightButtonOneClick.hand, RButtonOneClickDownCallback, RButtonOneClickPressedCallback, RButtonOneClickUpCallback);
            
            UpdateBoolButton(_rightButtonTwoTouch, _rightButtonTwoTouch.hand, RButtonTwoTouchDownCallback, RButtonTwoTouchPressedCallback, RButtonTwoTouchUpCallback);
            UpdateBoolButton(_rightButtonTwoClick, _rightButtonTwoClick.hand, RButtonTwoClickDownCallback, RButtonTwoClickPressedCallback, RButtonTwoClickUpCallback);
            
            
            // Menu Buttons
            UpdateBoolButton(_leftMenuButton, _leftMenuButton.hand, MenuButtonDownCallback, MenuButtonPressedCallback, MenuButtonUpCallback);
            
            
            // Joystick Touch
            UpdateBoolButton(_leftJoystickTouch, _leftJoystickTouch.hand, LJoystickTouchDownCallback, LJoystickTouchPressedCallback, LJoystickTouchUpCallback);
            UpdateBoolButton(_rightJoystickTouch, _rightJoystickTouch.hand, RJoystickTouchDownCallback, RJoystickTouchPressedCallback, RJoystickTouchUpCallback);
            
            
            // Joystick Click
            UpdateBoolButton(_leftJoystickClick, _leftJoystickClick.hand, LJoystickClickDownCallback, LJoystickClickPressedCallback, LJoystickClickUpCallback);
            UpdateBoolButton(_rightJoystickClick, _rightJoystickClick.hand, RJoystickClickDownCallback, RJoystickClickPressedCallback, RJoystickClickUpCallback);
            
            
            // Joystick Tilt
            UpdateVector2Button(_leftJoystickTilt, _leftJoystickTilt.hand, leftTiltThreshold, rightTiltThreshold, upTiltThreshold,
                downTiltThreshold, LJoystickTiltLeftCallback, LJoystickTiltLeftPressedCallback, LJoystickTiltRightCallback, 
                LJoystickTiltRightPressedCallback, LJoystickTiltUpCallback, LJoystickTiltUpPressedCallback, LJoystickTiltDownCallback,
                LJoystickTiltDownPressedCallback, LJoystickValueChangedCallback);
            
            UpdateVector2Button(_rightJoystickTilt, _rightTrigger.hand, leftTiltThreshold, rightTiltThreshold, upTiltThreshold,
                downTiltThreshold, RJoystickTiltLeftCallback, RJoystickTiltLeftPressedCallback, RJoystickTiltRightCallback, 
                RJoystickTiltRightPressedCallback, RJoystickTiltUpCallback, RJoystickTiltUpPressedCallback, RJoystickTiltDownCallback,
                RJoystickTiltDownPressedCallback, RJoystickValueChangedCallback);
        }

        private void UpdateBoolButton(Input boolInput,  Hand hand, InputEventHandler downEvent, InputEventHandler pressedEvent, InputEventHandler upEvent)
        {
            // Check if the button is being pressed at all
            if (!boolInput.device.TryGetFeatureValue(boolInput.boolInputUsage, out var boolReturn)) return;
            
            // Down Trigger Event
            if (!boolInput.boolValue && boolReturn)
            {
                boolInput.boolValue = true;
                    
                ButtonEvent(boolInput, hand, boolInput.boolInputUsage.name, downEvent, "Down");
            }
            
            // Pressed Event
            else if (boolInput.boolValue && boolReturn)
            {
                ButtonEvent(boolInput, hand, boolInput.boolInputUsage.name, pressedEvent, "Pressed");
            }
            
            // Up Trigger Event
            else if (boolInput.boolValue && !boolReturn)
            {
                boolInput.boolValue = false;

                ButtonEvent(boolInput, hand, boolInput.boolInputUsage.name, upEvent, "Up");
            }
            
        }
        
        private void UpdateVector1Button(Input vector1Input, Hand hand, float pressedDownThreshold, float pressedUpThreshold, 
            InputEventHandler downEvent, InputEventHandler pressedEvent, InputEventHandler upEvent, InputEventHandler valueChangedEvent) 
        {
            // Check if the button is being pressed at all
            if (!vector1Input.device.TryGetFeatureValue(vector1Input.boolInputUsage, out var boolReturn)) return;
            
            // Check if we get any value from the Vector1 button
            if (!vector1Input.device.TryGetFeatureValue(vector1Input.vector1InputUsage, out var vector1Return)) return;

            // Value Changed Event
            if (Math.Abs(vector1Input.vector1Value - vector1Return) > .01F)
            {
                vector1Input.vector1Value = vector1Return;

                ButtonEvent(vector1Input, hand, vector1Input.vector1InputUsage.name, valueChangedEvent, "Value Changed");
            }
            
            // Down Trigger Event
            if (vector1Return > pressedDownThreshold && !vector1Input.boolValue)
            {
                vector1Input.boolValue = true;
                    
                ButtonEvent(vector1Input, hand, vector1Input.vector1InputUsage.name, downEvent, "Down");
            }
            
            // Pressed Event
            else if (vector1Return > pressedDownThreshold)
            {
                ButtonEvent(vector1Input, hand, vector1Input.vector1InputUsage.name, pressedEvent, "Pressed");
            }
            
            // Up Trigger Event
            else if (vector1Return < pressedUpThreshold && vector1Input.boolValue)
            {
                vector1Input.boolValue = false;

                ButtonEvent(vector1Input, hand, vector1Input.vector1InputUsage.name, upEvent, "Up");
            }
        }

        private void UpdateVector2Button(Input vector2Input, Hand hand, float leftTiltThreshold, float rightTiltThreshold,
            float upTiltThreshold, float downTiltThreshold, InputEventHandler leftTiltEvent, InputEventHandler leftTiltPressedEvent,
            InputEventHandler rightTiltEvent, InputEventHandler rightTiltPressedEvent, InputEventHandler upTiltEvent, InputEventHandler upTiltPressedEvent,
            InputEventHandler downTiltEvent, InputEventHandler downTiltPressedEvent, InputEventHandler valueChangedEvent)
        {
            // Check if we get any value from the Vector1 button
            vector2Input.device.TryGetFeatureValue(vector2Input.vector2InputUsage, out var vector2Return);
            
            // Listen for changes in Joystick value
            if (Math.Abs(vector2Input.vector2Value.magnitude - vector2Return.magnitude) > .01F)
            {
                vector2Input.vector2Value = vector2Return;

                ButtonEvent(vector2Input, hand,vector2Input.vector2InputUsage.name, valueChangedEvent, "Value Changed");
            }
            
            // Left Tilt
            if (vector2Return.x < leftTiltThreshold)
            {
                // Left Tilt
                if (!vector2Input.leftTilt)
                {
                    vector2Input.leftTilt = true;
                
                    ButtonEvent(vector2Input, hand,vector2Input.vector2InputUsage.name, leftTiltEvent, "Left Tilt");    
                }
                
                // Left Tilt Pressed
                ButtonEvent(vector2Input, hand,vector2Input.vector2InputUsage.name, leftTiltPressedEvent, "Left Tilt Pressed");
            }
            else
            {
                vector2Input.leftTilt = false;
            }

            // Right Tilt
            if (vector2Return.x > rightTiltThreshold)
            {
                if (!vector2Input.rightTilt)
                {
                    vector2Input.rightTilt = true;
                    
                    ButtonEvent(vector2Input, hand,vector2Input.vector2InputUsage.name, rightTiltEvent, "Right Tilt");
                }
                
                ButtonEvent(vector2Input, hand,vector2Input.vector2InputUsage.name, rightTiltPressedEvent, "Right Tilt Pressed");
            }
            else
            {
                vector2Input.rightTilt = false;
            }
            
            // Up Tilt
            if (vector2Return.y > upTiltThreshold)
            {
                if (!vector2Input.upTilt)
                {
                    vector2Input.upTilt = true;
                    
                    ButtonEvent(vector2Input, hand, vector2Input.vector2InputUsage.name, upTiltEvent, "Up Tilt");
                }
                
                ButtonEvent(vector2Input, hand, vector2Input.vector2InputUsage.name, upTiltPressedEvent, "Up Tilt Pressed");
            }
            else
            {
                vector2Input.upTilt = false;
            }
            
            // Down Tilt
            if (vector2Return.y < downTiltThreshold)
            {
                if (!vector2Input.downTilt)
                {
                    vector2Input.downTilt = true;
                    
                    ButtonEvent(vector2Input, hand, vector2Input.vector2InputUsage.name, downTiltEvent, "Down Tilt");
                }
                
                ButtonEvent(vector2Input, hand, vector2Input.vector2InputUsage.name, downTiltPressedEvent, "Down Tilt Pressed");
            }
            else
            {
                vector2Input.downTilt = false;
            }
        }

        private void ButtonEvent(Input buttonInput, Hand hand, string buttonName, InputEventHandler inputEvent, string eventName)
        {
            var a = new InputEventArgs(hand);
            inputEvent?.DynamicInvoke(a);
            
            if (debug)
                Debug.Log(buttonInput.device.name + " " + buttonName + " " + eventName + " with " + hand.name);
        }
        
        // Constantly updates the position and rotation of the XRNode
        private void AssignInputDevices()
        {
            _rightDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            _leftDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        }
        
        private static void SetDevicePosAndRot(XRNode trackedDevice, GameObject hand)
        {
            var device = InputDevices.GetDeviceAtXRNode(trackedDevice);

            if (!device.isValid) return;

            device.TryGetFeatureValue(CommonUsages.devicePosition, out var position);
            device.TryGetFeatureValue(CommonUsages.deviceRotation, out var rotation);

            hand.transform.localRotation = rotation;
            hand.transform.localPosition = position;

            return;
        }
    }
    
    //Re-usable structure/ Can be a class to. Add all parameters you need inside it
    public struct EventParam
    {
        public Hand Hand;
    }

    [Serializable]
    public class EventItem
    {
        // public List<string> methodListeners = new List<string>();
        public Action<EventParam> action;
    }
    
    [Serializable]
    public class ButtonEventTest
    {
        public event PlayerInput.InputEventHandler Event;
        
        public ButtonEventTest(PlayerInput.InputEventHandler inputEvent)
        {
            this.inputEvent = inputEvent;
        }
        
        public PlayerInput.InputEventHandler inputEvent;
    }
}