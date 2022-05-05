//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Valve.VR.InteractionSystem; 
//public class ShowControllers : MonoBehaviour
//{
//    public bool showController = true;
//    public bool showHands = false;


//    private void Update()
//    {
//        foreach (var hand in Player.instance.hands)
//        {
//            if (showController)
//            {
//                hand.ShowController();
//                hand.SetSkeletonRangeOfMotion(Valve.VR.EVRSkeletalMotionRange.WithController);
//            }
//            else
//            {
//                hand.HideController();
//            }
//            if (showHands)
//            {
//                hand.ShowSkeleton();
//            }
//            else
//            {
//                hand.HideSkeleton();
//            }
//        }
//    }
//}
