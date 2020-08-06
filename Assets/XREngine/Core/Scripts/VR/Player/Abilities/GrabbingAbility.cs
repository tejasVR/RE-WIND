using UnityEngine;
using XRCORE.Scripts.VR.Interactive;
using XREngine.Core.Scripts.VR.Interactive;
using XREngine.Core.Scripts.VR.Player;
using XREngine.Core.Scripts.VR.Player.Abilities;

namespace XRCORE.Scripts.VR.Player
{
    public class GrabbingAbility : PlayerAbility
    {
        [Header("Grabbing Settings")]
        [SerializeField] protected bool hideHandOnGrab;
        [SerializeField] private float grabColliderRadius;
        [SerializeField] private GameObject handModel;

        private GrabbableItem _leftGrabbedItem;
        private GrabbableItem _rightGrabbedItem;

        protected override void PrimaryAction(InputEventArgs eventArgs)
        {
            switch (assignedPlayerComponent)
            {
                case AssignedPlayerComponent.DominantHand when eventArgs.Hand == PlayerManager.Instance.DominantHand:
                case AssignedPlayerComponent.NonDominantHand when eventArgs.Hand == PlayerManager.Instance.NonDominantHand:
                    Grab(eventArgs.Hand);
                    break;
                case AssignedPlayerComponent.BothHands:
                    Grab(eventArgs.Hand);
                    break;
                default:
                    break;
            }
        }

        protected override void SecondaryAction(InputEventArgs eventArgs)
        {
            switch (assignedPlayerComponent)
            {
                case AssignedPlayerComponent.DominantHand when eventArgs.Hand == PlayerManager.Instance.DominantHand:
                case AssignedPlayerComponent.NonDominantHand when eventArgs.Hand == PlayerManager.Instance.NonDominantHand:
                    Ungrab(eventArgs.Hand);
                    break;
                case AssignedPlayerComponent.BothHands:
                    Ungrab(eventArgs.Hand);
                    break;
                default:
                    break;
            }
        }

        private void Grab(Hand hand)
        {
            var maxColliders = 10;
            var hitColliders = new Collider[maxColliders];
            
            // Create a sphere that detects if there are any Grabbables in a specific radius
            var size = Physics.OverlapSphereNonAlloc(hand.transform.position, grabColliderRadius, hitColliders);

            if (debug) CreateDebugGrabSphere(hand);

            // For each collider hit, see if it's a Grabbable
            foreach (var go in hitColliders)
            {
                if (go == null) continue;
                
                // If this hand is already grabbing something, continue 
                if (IsHandGrabbingItem(hand)) continue;
                
                // If the collider we hit is not a Grabbable, continue
                if (!go.GetComponent<GrabbableItem>()) continue;
                
                var grabbedItem = go.GetComponent<GrabbableItem>();

                // If the grabbable we detected is grabbed by the other hand, switch the Grabbable to this hand
                if (grabbedItem == _leftGrabbedItem || grabbedItem == _rightGrabbedItem)
                {
                    // Get other hand
                    if (hand.Handedness == PlayerManager.Instance.DominantHand.Handedness)
                    {
                        SwitchObjectToOtherHand(PlayerManager.Instance.NonDominantHand, hand, grabbedItem);
                    }
                    else
                    {
                        SwitchObjectToOtherHand(PlayerManager.Instance.DominantHand, hand, grabbedItem);
                    }
                    
                    return;    
                }
                
                // Grab the detected Grabbable
                PutItemInHand(hand, grabbedItem);
                grabbedItem.GetComponent<InteractableItem>().OnItemSelected(hand);

                // PlayGrabSound(grabbableItem.GetGrabSFX());
                //
                // HapicPulse(.05F, .3F);

                // HideHand();
            }           
        }
        
        private void Ungrab(Hand hand)
        {
            if (!IsHandGrabbingItem(hand)) return;
            
            GetGrabbedItemOnHand(hand).OnItemUnselected(hand);

            ClearItemOnHand(hand);
                    
            // HapicPulse(.05F, .1F);

            // ShowHand();
        }        
        
        private void CreateDebugGrabSphere(Hand hand)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.localScale = new Vector3(grabColliderRadius, grabColliderRadius, grabColliderRadius);
            sphere.transform.position = hand.transform.position;

            Destroy(sphere, 4F);
        }
        
        private void SwitchObjectToOtherHand(Hand handItemIsIn, Hand handToSwitchTo, GrabbableItem grabbedItem)
        {
            grabbedItem.OnItemSelected(handToSwitchTo);
            
            ClearItemOnHand(handItemIsIn);
            
            PutItemInHand(handToSwitchTo, grabbedItem);
            // ShowHand();
            //grabbableItem = null;
        }
        
        public virtual void HideHand()
        {
            if (hideHandOnGrab)
            {
                handModel.SetActive(false);
            }
        }

        public virtual void ShowHand()
        {
            if (hideHandOnGrab)
            {
                handModel.SetActive(true);
            }
        }

        private bool IsHandGrabbingItem(Hand hand)
        {
            return hand.Handedness == Hand.HandOrientation.Left
                ? _leftGrabbedItem != null
                : _rightGrabbedItem != null;
        }

        private GrabbableItem GetGrabbedItemOnHand(Hand hand)
        {
            return hand.Handedness == Hand.HandOrientation.Left
                ? _leftGrabbedItem
                : _rightGrabbedItem;
        }

        private void PutItemInHand(Hand hand, GrabbableItem grabbedItem)
        {
            if (hand.Handedness == Hand.HandOrientation.Left)
            {
                _leftGrabbedItem = grabbedItem;
            }
            else
            {
                _rightGrabbedItem = grabbedItem;
            }
        }

        private void ClearItemOnHand(Hand hand)
        {
            if (hand.Handedness == Hand.HandOrientation.Left)
            {
                _leftGrabbedItem = null;
            }
            else
            {
                _rightGrabbedItem = null;
            }
        }
    }
}
