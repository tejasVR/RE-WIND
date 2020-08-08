using System;
using Sirenix.OdinInspector;
using UnityEngine;
using XRCORE.Scripts.VR.Player;

namespace XREngine.Core.Scripts.VR.Player
{
    // A script that manages player object

    public class PlayerManager : MonoBehaviour
    {
        public static event Action SwitchDominantHandCallback = delegate { };

        public static PlayerManager Instance
        {
            get
            {
                if (_playerManager) return _playerManager;
                
                _playerManager = FindObjectOfType<PlayerManager>();

                if (!_playerManager)
                {
                    Debug.LogError("There needs to be a PlayerManager in the scene!");
                }
                else
                {
                    _playerManager.Initialize();
                }

                return _playerManager;
            }
        }

        private static PlayerManager _playerManager;

        [SerializeField] private bool debug;
        
        public Hand DominantHand { get; private set; }
        public Hand NonDominantHand { get; private set; }

        public Head PlayerHead => playerHead;
        public Hand LeftHand => leftHand;
        public Hand RightHand => rightHand;

        [SerializeField] private Head playerHead;
        [SerializeField] private Hand leftHand;
        [SerializeField] private Hand rightHand;
        
        public enum SpawnType
        {
            [InspectorName("Floor")]  Floor,
            [InspectorName("Eye Level (Default)")] EyeLevel
        }
        
        [Space(7)]
        [SerializeField]
        private SpawnType spawnType;
        
        [HorizontalGroup("Split", 0.5F), PropertySpace]
        [Button(ButtonSizes.Medium)]
        private void SetLeftHandDominant()
        {
            SetDominantHand(Hand.HandOrientation.Left);
        }
        
        [HorizontalGroup("Split", 0.5F), PropertySpace]
        [Button(ButtonSizes.Medium)]
        private void SetRightHandDominant()
        {
            SetDominantHand(Hand.HandOrientation.Right);
        }
        
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

        private void Awake()
        {
            GetDominantHand();

            CheckDeployedDevice();
        }

        private void Start()
        {
            SetSpawnPosition();
        }
        
        private void Initialize()
        {
            _playerManager = this;
        }

        public void SetPlayerPosition(Transform positionTransform, bool changeHeight)
        {
            if (changeHeight)
            {
                transform.position = positionTransform.position;
            }
            else
            {
                var position = positionTransform.position;
                transform.position = new Vector3(position.x, transform.position.y, position.z);
            }
        }

        public void SetPlayerRotation(Quaternion rotationTransform)
        {
            var yAngleDifference = (rotationTransform.eulerAngles.y - PlayerHead.transform.eulerAngles.y);
            
            var rotation = transform.eulerAngles;

            rotation.y += yAngleDifference;

            transform.eulerAngles = rotation;
        }

        public void ResetCameraTransform()
        {
            var rotation = PlayerHead.transform.rotation;

            rotation.z = 0;
            rotation.x = 0;

            transform.rotation = rotation;
        }

        public void SetPlayerHeight(float newHeight)
        {
            var position = transform.position;
            position = new Vector3(position.x, newHeight, position.z);
            transform.position = position;
        }
        
        private void SetSpawnPosition()
        {
            switch (spawnType)
            {
                case SpawnType.Floor:
                    transform.position = new Vector3(transform.position.x, 0F, transform.position.z);
                    break;
            }
        }

        private void GetDominantHand()
        {
            var handedness = OVRInput.GetDominantHand();

            DominantHand = handedness == OVRInput.Handedness.LeftHanded ? LeftHand : RightHand;

            SetDominantHand(DominantHand == LeftHand ? Hand.HandOrientation.Left : Hand.HandOrientation.Right);
        }

        private void SetDominantHand(Hand.HandOrientation dominantHandOrientation)
        {
            DominantHand = dominantHandOrientation == Hand.HandOrientation.Right ? RightHand : LeftHand;
            NonDominantHand = dominantHandOrientation == Hand.HandOrientation.Right ? LeftHand : RightHand;
            
            if (debug) Debug.Log("Dominant Hand: " + DominantHand.Handedness.ToString() + " Hand");
            
            SwitchDominantHandCallback?.Invoke();
        }
        
     

        private void CheckDeployedDevice()
        {
            // if (OVRPlugin.GetSystemHeadsetType() == OVRPlugin.SystemHeadset.Oculus_Go)
            // {
            //     if (DominantHand == LeftHand)
            //     {
            //         RightHand.gameObject.SetActive(false);
            //     }
            //     else
            //     {
            //         LeftHand.gameObject.SetActive(false);
            //     }
            // }

            if (OVRPlugin.GetSystemHeadsetType() == OVRPlugin.SystemHeadset.Rift_S)
            {
            }

            if (OVRPlugin.GetSystemHeadsetType() == OVRPlugin.SystemHeadset.Oculus_Quest)
            {
            }
        }

        public Transform GetHeadTransform()
        {
            return PlayerHead.transform;
        }
    }
}