﻿using System.Collections.Generic;
using UnityEngine;


namespace RoadArchitect
{
    public static class IntersectionObjects
    {
        public static void CleanupIntersectionObjects(GameObject _masterGameObj)
        {
            int childCount = _masterGameObj.transform.childCount;
            if (childCount == 0)
            {
                return;
            }
            List<GameObject> objectsToDelete = new List<GameObject>();
            for (int index = 0; index < childCount; index++)
            {
                if (_masterGameObj.transform.GetChild(index).name.ToLower().Contains("stopsign"))
                {
                    objectsToDelete.Add(_masterGameObj.transform.GetChild(index).gameObject);
                }
                if (_masterGameObj.transform.GetChild(index).name.ToLower().Contains("trafficlight"))
                {
                    objectsToDelete.Add(_masterGameObj.transform.GetChild(index).gameObject);
                }
            }
            for (int index = (objectsToDelete.Count - 1); index >= 0; index--)
            {
                Object.DestroyImmediate(objectsToDelete[index]);
            }
        }


        #region "Stop Sign All Way"
        public static void CreateStopSignsAllWay(GameObject _masterGameObj, bool _isRB = true)
        {
            CreateStopSignsAllWayDo(ref _masterGameObj, _isRB);
        }


        private static void TFixSigns(GameObject _obj)
        {
            Rigidbody rigidbody = _obj.GetComponent<Rigidbody>();
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
        }


        private static void CreateStopSignsAllWayDo(ref GameObject _masterGameObj, bool _isRB)
        {
            Object prefab = UnityEditor.AssetDatabase.LoadAssetAtPath(RoadEditorUtility.GetBasePath() + "/Mesh/RoadObj/Signs/GSDSignStopAllway.prefab", typeof(GameObject));

            RoadIntersection GSDRI = _masterGameObj.GetComponent<RoadIntersection>();
            SplineC tSpline = GSDRI.node1.spline;

            GameObject tObj = null;
            //			Vector3 xDir = default(Vector3);
            Vector3 tDir = default(Vector3);
            //			float RoadWidth = tSpline.tRoad.RoadWidth();
            //			float LaneWidth = tSpline.tRoad.opt_LaneWidth;
            float ShoulderWidth = tSpline.road.shoulderWidth;

            //Cleanup:
            CleanupIntersectionObjects(_masterGameObj);

            float Mass = 100f;

            //Get four points:
            float DistFromCorner = (ShoulderWidth * 0.45f);
            Vector3 tPosRR = default(Vector3);
            Vector3 tPosRL = default(Vector3);
            Vector3 tPosLR = default(Vector3);
            Vector3 tPosLL = default(Vector3);
            GetFourPoints(GSDRI, out tPosRR, out tPosRL, out tPosLL, out tPosLR, DistFromCorner);

            //RR:
            tSpline = GSDRI.node1.spline;
            tObj = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
            //			xDir = (GSDRI.CornerRR - GSDRI.transform.position).normalized;
            tDir = StopSignGetRotRR(GSDRI, tSpline);
            tObj.transform.rotation = Quaternion.LookRotation(tDir) * Quaternion.Euler(0f, 180f, 0f);
            if (_isRB)
            {
                Rigidbody RB = tObj.AddComponent<Rigidbody>();
                RB.mass = Mass;
                RB.centerOfMass = new Vector3(0f, -10f, 0f);
            }
            tObj.transform.parent = _masterGameObj.transform;
            tObj.transform.position = tPosRR;
            tObj.name = "StopSignRR";
            TFixSigns(tObj);
            if (GSDRI.ignoreCorner == 0)
            {
                Object.DestroyImmediate(tObj);
            }

            //LL:
            tSpline = GSDRI.node1.spline;
            tObj = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
            //			xDir = (GSDRI.CornerLL - GSDRI.transform.position).normalized;
            tDir = StopSignGetRotLL(GSDRI, tSpline);
            tObj.transform.rotation = Quaternion.LookRotation(tDir) * Quaternion.Euler(0f, 180f, 0f);
            if (_isRB)
            {
                Rigidbody RB = tObj.AddComponent<Rigidbody>();
                RB.mass = Mass;
                RB.centerOfMass = new Vector3(0f, -10f, 0f);
            }
            tObj.transform.parent = _masterGameObj.transform;
            tObj.transform.position = tPosLL;
            tObj.name = "StopSignLL";
            TFixSigns(tObj);
            if (GSDRI.ignoreCorner == 2)
            {
                Object.DestroyImmediate(tObj);
            }

            //RL:
            tSpline = GSDRI.node2.spline;
            tObj = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
            //			xDir = (GSDRI.CornerRL - GSDRI.transform.position).normalized;
            tDir = StopSignGetRotRL(GSDRI, tSpline);
            tObj.transform.rotation = Quaternion.LookRotation(tDir) * Quaternion.Euler(0f, 180f, 0f);
            if (_isRB)
            {
                Rigidbody RB = tObj.AddComponent<Rigidbody>();
                RB.mass = Mass;
                RB.centerOfMass = new Vector3(0f, -10f, 0f);
            }
            tObj.transform.parent = _masterGameObj.transform;
            tObj.transform.position = tPosRL;
            tObj.name = "StopSignRL";
            TFixSigns(tObj);
            if (GSDRI.ignoreCorner == 1)
            {
                Object.DestroyImmediate(tObj);
            }

            //LR:
            tSpline = GSDRI.node2.spline;
            tObj = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
            //			xDir = (GSDRI.CornerLR - GSDRI.transform.position).normalized;
            tDir = StopSignGetRotLR(GSDRI, tSpline);
            tObj.transform.rotation = Quaternion.LookRotation(tDir) * Quaternion.Euler(0f, 180f, 0f);
            if (_isRB)
            {
                Rigidbody RB = tObj.AddComponent<Rigidbody>();
                RB.mass = Mass;
                RB.centerOfMass = new Vector3(0f, -10f, 0f);
            }
            tObj.transform.parent = _masterGameObj.transform;
            tObj.transform.position = tPosLR;
            tObj.name = "StopSignLR";
            TFixSigns(tObj);
            if (GSDRI.ignoreCorner == 3)
            {
                Object.DestroyImmediate(tObj);
            }
        }


        private static Vector3 StopSignGetRotRR(RoadIntersection _intersection, SplineC _spline)
        {
            float tDist = ((Vector3.Distance(_intersection.cornerRL, _intersection.cornerRR) / 2f) + (0.025f * Vector3.Distance(_intersection.cornerLL, _intersection.cornerRR))) / _spline.distance;
            float p = Mathf.Clamp(_intersection.node1.time - tDist, 0f, 1f);
            Vector3 POS = _spline.GetSplineValue(p, true);
            return (POS * -1);
        }


        private static Vector3 StopSignGetRotLL(RoadIntersection _intersection, SplineC _spline)
        {
            float tDist = ((Vector3.Distance(_intersection.cornerLR, _intersection.cornerLL) / 2f) + (0.025f * Vector3.Distance(_intersection.cornerLL, _intersection.cornerRR))) / _spline.distance;
            float p = Mathf.Clamp(_intersection.node1.time + tDist, 0f, 1f);
            Vector3 POS = _spline.GetSplineValue(p, true);
            return POS;
        }


        private static Vector3 StopSignGetRotRL(RoadIntersection _intersetion, SplineC _spline)
        {
            float tDist = ((Vector3.Distance(_intersetion.cornerLL, _intersetion.cornerRL) / 2f) + (0.025f * Vector3.Distance(_intersetion.cornerLR, _intersetion.cornerRL))) / _spline.distance;
            float p = -1f;
            if (_intersetion.isFlipped)
            {
                p = Mathf.Clamp(_intersetion.node2.time - tDist, 0f, 1f);
            }
            else
            {
                p = Mathf.Clamp(_intersetion.node2.time + tDist, 0f, 1f);
            }
            Vector3 POS = _spline.GetSplineValue(p, true);
            //POS = Vector3.Cross(POS,Vector3.up);
            if (_intersetion.isFlipped)
            {
                return (POS * -1);
            }
            else
            {
                return POS;
            }
        }


        private static Vector3 StopSignGetRotLR(RoadIntersection _intersection, SplineC _spline)
        {
            float tDist = ((Vector3.Distance(_intersection.cornerRR, _intersection.cornerLR) / 2f) + (0.025f * Vector3.Distance(_intersection.cornerLR, _intersection.cornerRL))) / _spline.distance;
            float p = -1f;
            if (_intersection.isFlipped)
            {
                p = Mathf.Clamp(_intersection.node2.time + tDist, 0f, 1f);
            }
            else
            {
                p = Mathf.Clamp(_intersection.node2.time - tDist, 0f, 1f);
            }
            Vector3 POS = _spline.GetSplineValue(p, true);
            //POS = Vector3.Cross(POS,Vector3.up);
            if (_intersection.isFlipped)
            {
                return POS;
            }
            else
            {
                return (POS * -1);
            }
        }
        #endregion


        #region "Traffic light bases"
        public static void CreateTrafficLightBases(GameObject _masterGameObj, bool _isTrafficLight1 = true)
        {
            CreateTrafficLightBases_Do(ref _masterGameObj, _isTrafficLight1);
        }


        private static void CreateTrafficLightBases_Do(ref GameObject _masterGameObj, bool _isTrafficLight1)
        {
            RoadIntersection intersection = _masterGameObj.GetComponent<RoadIntersection>();
            SplineC spline = intersection.node1.spline;
            bool isRB = true;

            //float RoadWidth = tSpline.tRoad.RoadWidth();
            float LaneWidth = spline.road.laneWidth;
            float ShoulderWidth = spline.road.shoulderWidth;

            int Lanes = spline.road.laneAmount;
            int LanesHalf = Lanes / 2;
            float LanesForInter = -1;
            if (intersection.roadType == RoadIntersection.RoadTypeEnum.BothTurnLanes)
            {
                LanesForInter = LanesHalf + 1f;
            }
            else if (intersection.roadType == RoadIntersection.RoadTypeEnum.TurnLane)
            {
                LanesForInter = LanesHalf + 1f;
            }
            else
            {
                LanesForInter = LanesHalf - 1 + 1f;
            }
            float DistFromCorner = (ShoulderWidth * 0.45f);
            float TLDistance = (LanesForInter * LaneWidth) + DistFromCorner;

            GameObject tObjRR = null;
            GameObject tObjRL = null;
            GameObject tObjLL = null;
            GameObject tObjLR = null;
            //			Vector3 xDir = default(Vector3);
            Vector3 tDir = default(Vector3);
            float Mass = 12500f;
            Vector3 COM = new Vector3(0f, 0f, 4f);
            Vector3 zeroVect = new Vector3(0f, 0f, 0f);
            Vector3 StartVec = default(Vector3);
            Vector3 EndVec = default(Vector3);
            //			bool bContains = false;
            //			MeshFilter MF = null;
            //			Vector3[] tVerts = null;
            Rigidbody RB = null;

            //Get four points:
            Vector3 tPosRR = default(Vector3);
            Vector3 tPosRL = default(Vector3);
            Vector3 tPosLR = default(Vector3);
            Vector3 tPosLL = default(Vector3);
            GetFourPoints(intersection, out tPosRR, out tPosRL, out tPosLL, out tPosLR, DistFromCorner);

            //Cleanup:
            CleanupIntersectionObjects(_masterGameObj);

            float[] tempDistances = new float[4];
            tempDistances[0] = Vector3.Distance(intersection.cornerRL, intersection.cornerLL);
            tempDistances[1] = Vector3.Distance(intersection.cornerRL, intersection.cornerRR);
            tempDistances[2] = Vector3.Distance(intersection.cornerLR, intersection.cornerLL);
            tempDistances[3] = Vector3.Distance(intersection.cornerLR, intersection.cornerRR);
            float MaxDistanceStart = Mathf.Max(tempDistances);
            bool OrigPoleAlignment = intersection.isRegularPoleAlignment;

            //Node1:
            //RL:
            tObjRL = CreateTrafficLight(TLDistance, true, true, MaxDistanceStart, intersection.isTrafficPoleStreetLight, spline.road.GSDRS.isSavingMeshes);
            //			xDir = (GSDRI.CornerRL - GSDRI.transform.position).normalized;
            tDir = TrafficLightBaseGetRotRL(intersection, spline, DistFromCorner);
            if (tDir == zeroVect)
            {
                tDir = new Vector3(0f, 0.0001f, 0f);
            }
            tObjRL.transform.rotation = Quaternion.LookRotation(tDir) * Quaternion.Euler(-90f, -180f, 0f);
            tObjRL.transform.parent = _masterGameObj.transform;
            StartVec = tPosRL;
            EndVec = (tDir.normalized * TLDistance) + StartVec;
            if (!intersection.isRegularPoleAlignment && intersection.ContainsLine(StartVec, EndVec))
            { //Convert to regular alignment if necessary
                tObjRL.transform.parent = null;
                tDir = TrafficLightBaseGetRotRL(intersection, spline, DistFromCorner, true);
                if (tDir == zeroVect)
                {
                    tDir = new Vector3(0f, 0.0001f, 0f);
                }
                tObjRL.transform.rotation = Quaternion.LookRotation(tDir) * Quaternion.Euler(-90f, -180f, 0f);
                tObjRL.transform.parent = _masterGameObj.transform;
            }
            else
            {
                intersection.isRegularPoleAlignment = true;
                if (tObjRL != null)
                {
                    Object.DestroyImmediate(tObjRL);
                }
                tObjRL = CreateTrafficLight(TLDistance, true, true, MaxDistanceStart, intersection.isTrafficPoleStreetLight, spline.road.GSDRS.isSavingMeshes);
                //				xDir = (GSDRI.CornerRL - GSDRI.transform.position).normalized;
                tDir = TrafficLightBaseGetRotRL(intersection, spline, DistFromCorner);
                if (tDir == zeroVect)
                {
                    tDir = new Vector3(0f, 0.0001f, 0f);
                }
                tObjRL.transform.rotation = Quaternion.LookRotation(tDir) * Quaternion.Euler(-90f, -180f, 0f);
                tObjRL.transform.parent = _masterGameObj.transform;
                StartVec = tPosRL;
                EndVec = (tDir.normalized * TLDistance) + StartVec;
                intersection.isRegularPoleAlignment = OrigPoleAlignment;
            }
            if (isRB)
            {
                RB = tObjRL.AddComponent<Rigidbody>();
                RB.mass = Mass;
                RB.centerOfMass = COM;
                tObjRL.AddComponent<RoadArchitect.RigidBody>();
            }
            tObjRL.transform.position = tPosRL;
            tObjRL.transform.name = "TrafficLightRL";
            //LR:
            tObjLR = CreateTrafficLight(TLDistance, true, true, MaxDistanceStart, intersection.isTrafficPoleStreetLight, spline.road.GSDRS.isSavingMeshes);
            //xDir = (GSDRI.CornerLR - GSDRI.transform.position).normalized;
            tDir = TrafficLightBaseGetRotLR(intersection, spline, DistFromCorner);
            if (tDir == zeroVect)
            {
                tDir = new Vector3(0f, 0.0001f, 0f);
            }
            tObjLR.transform.rotation = Quaternion.LookRotation(tDir) * Quaternion.Euler(-90f, -180f, 0f);
            tObjLR.transform.parent = _masterGameObj.transform;
            StartVec = tPosLR;
            EndVec = (tDir.normalized * TLDistance) + StartVec;
            if (!intersection.isRegularPoleAlignment && intersection.ContainsLine(StartVec, EndVec))
            { //Convert to regular alignment if necessary
                tObjLR.transform.parent = null;
                tDir = TrafficLightBaseGetRotLR(intersection, spline, DistFromCorner, true);
                if (tDir == zeroVect)
                {
                    tDir = new Vector3(0f, 0.0001f, 0f);
                }
                tObjLR.transform.rotation = Quaternion.LookRotation(tDir) * Quaternion.Euler(-90f, -180f, 0f);
                tObjLR.transform.parent = _masterGameObj.transform;
            }
            else
            {
                intersection.isRegularPoleAlignment = true;
                if (tObjLR != null)
                {
                    Object.DestroyImmediate(tObjLR);
                }
                tObjLR = CreateTrafficLight(TLDistance, true, true, MaxDistanceStart, intersection.isTrafficPoleStreetLight, spline.road.GSDRS.isSavingMeshes);
                //				xDir = (GSDRI.CornerLR - GSDRI.transform.position).normalized;
                tDir = TrafficLightBaseGetRotLR(intersection, spline, DistFromCorner);
                if (tDir == zeroVect)
                {
                    tDir = new Vector3(0f, 0.0001f, 0f);
                }
                tObjLR.transform.rotation = Quaternion.LookRotation(tDir) * Quaternion.Euler(-90f, -180f, 0f);
                tObjLR.transform.parent = _masterGameObj.transform;
                StartVec = tPosLR;
                EndVec = (tDir.normalized * TLDistance) + StartVec;
                intersection.isRegularPoleAlignment = OrigPoleAlignment;
            }
            if (isRB)
            {
                RB = tObjLR.AddComponent<Rigidbody>();
                RB.mass = Mass;
                RB.centerOfMass = COM;
                tObjLR.AddComponent<RoadArchitect.RigidBody>();
            }
            tObjLR.transform.position = tPosLR;
            tObjLR.transform.name = "TrafficLightLR";
            //Node2:
            //RR:
            tObjRR = CreateTrafficLight(TLDistance, true, true, MaxDistanceStart, intersection.isTrafficPoleStreetLight, spline.road.GSDRS.isSavingMeshes);
            //			xDir = (GSDRI.CornerRR - GSDRI.transform.position).normalized;
            tDir = TrafficLightBaseGetRotRR(intersection, spline, DistFromCorner);
            if (tDir == zeroVect)
            {
                tDir = new Vector3(0f, 0.0001f, 0f);
            }
            tObjRR.transform.rotation = Quaternion.LookRotation(tDir) * Quaternion.Euler(-90f, -180f, 0f);
            tObjRR.transform.parent = _masterGameObj.transform;
            StartVec = tPosRR;
            EndVec = (tDir.normalized * TLDistance) + StartVec;
            if (!intersection.isRegularPoleAlignment && intersection.ContainsLine(StartVec, EndVec))
            { //Convert to regular alignment if necessary
                tObjRR.transform.parent = null;
                tDir = TrafficLightBaseGetRotRR(intersection, spline, DistFromCorner, true);
                if (tDir == zeroVect)
                {
                    tDir = new Vector3(0f, 0.0001f, 0f);
                }
                tObjRR.transform.rotation = Quaternion.LookRotation(tDir) * Quaternion.Euler(-90f, 0f, 0f);
                tObjRR.transform.parent = _masterGameObj.transform;
            }
            else
            {
                intersection.isRegularPoleAlignment = true;
                if (tObjRR != null)
                { Object.DestroyImmediate(tObjRR); }
                tObjRR = CreateTrafficLight(TLDistance, true, true, MaxDistanceStart, intersection.isTrafficPoleStreetLight, spline.road.GSDRS.isSavingMeshes);
                //				xDir = (GSDRI.CornerRR - GSDRI.transform.position).normalized;
                tDir = TrafficLightBaseGetRotRR(intersection, spline, DistFromCorner);
                if (tDir == zeroVect)
                {
                    tDir = new Vector3(0f, 0.0001f, 0f);
                }
                tObjRR.transform.rotation = Quaternion.LookRotation(tDir) * Quaternion.Euler(-90f, -180f, 0f);
                tObjRR.transform.parent = _masterGameObj.transform;
                StartVec = tPosRR;
                EndVec = (tDir.normalized * TLDistance) + StartVec;
                intersection.isRegularPoleAlignment = OrigPoleAlignment;
            }
            if (isRB)
            {
                RB = tObjRR.AddComponent<Rigidbody>();
                RB.mass = Mass;
                RB.centerOfMass = COM;
                tObjRR.AddComponent<RoadArchitect.RigidBody>();
            }
            tObjRR.transform.position = tPosRR;
            tObjRR.transform.name = "TrafficLightRR";

            //LL:
            tObjLL = CreateTrafficLight(TLDistance, true, true, MaxDistanceStart, intersection.isTrafficPoleStreetLight, spline.road.GSDRS.isSavingMeshes);
            //			xDir = (GSDRI.CornerLL - GSDRI.transform.position).normalized;
            tDir = TrafficLightBaseGetRotLL(intersection, spline, DistFromCorner);
            if (tDir == zeroVect)
            {
                tDir = new Vector3(0f, 0.0001f, 0f);
            }
            tObjLL.transform.rotation = Quaternion.LookRotation(tDir) * Quaternion.Euler(-90f, -180f, 0f);
            tObjLL.transform.parent = _masterGameObj.transform;
            StartVec = tPosLL;
            EndVec = (tDir.normalized * TLDistance) + StartVec;
            if (!intersection.isRegularPoleAlignment && intersection.ContainsLine(StartVec, EndVec))
            { //Convert to regular alignment if necessary
                tObjLL.transform.parent = null;
                tDir = TrafficLightBaseGetRotLL(intersection, spline, DistFromCorner, true);
                if (tDir == zeroVect)
                {
                    tDir = new Vector3(0f, 0.0001f, 0f);
                }
                tObjLL.transform.rotation = Quaternion.LookRotation(tDir) * Quaternion.Euler(-90f, 0f, 0f);
                tObjLL.transform.parent = _masterGameObj.transform;
            }
            else
            {
                intersection.isRegularPoleAlignment = true;
                if (tObjLL != null)
                {
                    Object.DestroyImmediate(tObjLL);
                }
                tObjLL = CreateTrafficLight(TLDistance, true, true, MaxDistanceStart, intersection.isTrafficPoleStreetLight, spline.road.GSDRS.isSavingMeshes);
                //				xDir = (GSDRI.CornerLL - GSDRI.transform.position).normalized;
                tDir = TrafficLightBaseGetRotLL(intersection, spline, DistFromCorner);
                if (tDir == zeroVect)
                {
                    tDir = new Vector3(0f, 0.0001f, 0f);
                }
                tObjLL.transform.rotation = Quaternion.LookRotation(tDir) * Quaternion.Euler(-90f, -180f, 0f);
                tObjLL.transform.parent = _masterGameObj.transform;
                StartVec = tPosLL;
                EndVec = (tDir.normalized * TLDistance) + StartVec;
                intersection.isRegularPoleAlignment = OrigPoleAlignment;
            }
            if (isRB)
            {
                RB = tObjLL.AddComponent<Rigidbody>();
                RB.mass = Mass;
                RB.centerOfMass = COM;
                tObjLL.AddComponent<RoadArchitect.RigidBody>();
            }
            tObjLL.transform.position = tPosLL;
            tObjLL.transform.name = "TrafficLightLL";

            //Create the actual lights:
            CreateTrafficLightMains(_masterGameObj, tObjRR, tObjRL, tObjLL, tObjLR);
        }


        private static bool CreateTrafficLightBase_IsInIntersection(RoadIntersection _intersection, ref Vector3 _startVec, ref Vector3 _endVec)
        {
            return _intersection.ContainsLine(_startVec, _endVec);
        }


        private static GameObject CreateTrafficLight(float _distance, bool _isTrafficLight1, bool _isOptionalCollider, float _distanceX, bool _isLight, bool _isSavingAsset)
        {
            GameObject tObj = null;
            string tTrafficLightNumber = "1";
            if (!_isTrafficLight1)
            {
                tTrafficLightNumber = "2";
            }

            bool bDoCustom = false;
            //Round up to nearest whole F
            _distanceX = Mathf.Ceil(_distanceX);
            _distance = Mathf.Ceil(_distance);
            //string assetName = "GSDInterTLB" + tTrafficLightNumber + "_" + tDistance.ToString("F0") + "_" + xDistance.ToString("F0") + ".prefab";
            string assetNameAsset = "GSDInterTLB" + tTrafficLightNumber + "_" + _distance.ToString("F0") + "_" + _distanceX.ToString("F0") + ".asset";
            string BackupFBX = "GSDInterTLB" + tTrafficLightNumber + ".FBX";
            float tMod = _distance / 5f;
            float hMod = (_distance / 10f) * 0.7f;
            float xMod = ((_distanceX / 20f) + 2f) * 0.3334f;
            xMod = Mathf.Clamp(xMod, 1f, 20f);
            tMod = Mathf.Clamp(tMod, 1f, 20f);
            hMod = Mathf.Clamp(hMod, 1f, 20f);

            bool bXMod = false;
            if (!RootUtils.IsApproximately(xMod, 1f, 0.0001f))
            {
                bXMod = true;
            }

            string basePath = RoadEditorUtility.GetBasePath();

            Mesh xMesh = (Mesh)UnityEditor.AssetDatabase.LoadAssetAtPath(basePath + "/Mesh/RoadObj/Signs/TrafficLightBases/" + assetNameAsset, typeof(Mesh));
            if (xMesh == null)
            {
                xMesh = (Mesh)UnityEditor.AssetDatabase.LoadAssetAtPath(basePath + "/Mesh/RoadObj/Signs/TrafficLightBases/" + BackupFBX, typeof(Mesh));
                bDoCustom = true;
            }

            tObj = new GameObject("TempTrafficLight");
            MeshFilter MF = tObj.GetComponent<MeshFilter>();
            if (MF == null)
            {
                MF = tObj.AddComponent<MeshFilter>();
            }
            MeshRenderer MR = tObj.GetComponent<MeshRenderer>();
            if (MR == null)
            {
                MR = tObj.AddComponent<MeshRenderer>();
            }
            RoadEditorUtility.SetRoadMaterial(basePath + "/Materials/Signs/GSDInterTLB" + tTrafficLightNumber + ".mat", MR);

            if (!bDoCustom)
            {
                MF.sharedMesh = xMesh;
            }

            float tempMaxHeight = 7.6f * hMod;
            float xValue = tempMaxHeight - 7.6f;
            if (bDoCustom)
            {
                Mesh tMesh = new Mesh();
                tMesh.vertices = xMesh.vertices;
                tMesh.triangles = xMesh.triangles;
                tMesh.uv = xMesh.uv;
                tMesh.normals = xMesh.normals;
                tMesh.tangents = xMesh.tangents;
                MF.sharedMesh = tMesh;
                Vector3[] tVerts = tMesh.vertices;

                xValue = (xMod * 6f) - 6f;
                if ((xMod * 6f) > (tempMaxHeight - 1f))
                {
                    xValue = (tempMaxHeight - 1f) - 6f;
                }

                //				float tValue = 0f;
                //				float hValue = 0f;
                bool bIgnoreMe = false;


                int mCount = tVerts.Length;
                Vector2[] uv = tMesh.uv;
                //				List<int> tUVInts = new List<int>();
                for (int index = 0; index < mCount; index++)
                {
                    bIgnoreMe = false;
                    if (RootUtils.IsApproximately(tVerts[index].y, 5f, 0.01f))
                    {
                        tVerts[index].y = _distance;
                        if (uv[index].y > 3.5f)
                        {
                            uv[index].y *= tMod;
                        }
                        bIgnoreMe = true;
                    }
                    if (!bIgnoreMe && tVerts[index].z > 7.5f)
                    {
                        tVerts[index].z *= hMod;
                        if (uv[index].y > 3.8f)
                        {
                            uv[index].y *= hMod;
                        }
                    }

                    if (bXMod && tVerts[index].z > 4.8f && tVerts[index].z < 6.2f)
                    {
                        tVerts[index].z += xValue;
                    }
                }
                tMesh.vertices = tVerts;
                tMesh.uv = uv;
                tMesh.RecalculateNormals();
                tMesh.RecalculateBounds();

                //Save:
                if (_isSavingAsset)
                {
                    UnityEditor.AssetDatabase.CreateAsset(tMesh, basePath + "/Mesh/RoadObj/Signs/TrafficLightBases/" + assetNameAsset);
                }
            }

            BoxCollider BC = tObj.AddComponent<BoxCollider>();
            float MaxHeight = MF.sharedMesh.vertices[447].z;
            BC.size = new Vector3(0.35f, 0.35f, MaxHeight);
            BC.center = new Vector3(0f, 0f, (MaxHeight / 2f));

            if (_isOptionalCollider)
            {
                float MaxWidth = MF.sharedMesh.vertices[497].y;
                GameObject tObjCollider = new GameObject("col2");
                BC = tObjCollider.AddComponent<BoxCollider>();
                BC.size = new Vector3(0.175f, MaxWidth, 0.175f);
                BC.center = new Vector3(0f, MaxWidth / 2f, 5.808f);
                tObjCollider.transform.parent = tObj.transform;
            }

            if (_isLight)
            {
                GameObject yObj = (GameObject)UnityEditor.AssetDatabase.LoadAssetAtPath(basePath + "/Mesh/RoadObj/Signs/GSDStreetLight_TrafficLight.prefab", typeof(GameObject));
                GameObject kObj = (GameObject)GameObject.Instantiate(yObj);
                kObj.transform.position = tObj.transform.position;
                kObj.transform.position += new Vector3(0f, 0f, MaxHeight - 7.6f);
                kObj.transform.parent = tObj.transform;
                kObj.transform.rotation = Quaternion.identity;
                //				kObj.name = "StreetLight";
            }


            //Bounds calcs:
            MeshFilter[] tMeshes = tObj.GetComponents<MeshFilter>();
            for (int index = 0; index < tMeshes.Length; index++)
            {
                tMeshes[index].sharedMesh.RecalculateBounds();
            }

            return tObj;
        }


        private static Vector3 TrafficLightBaseGetRotRL(RoadIntersection _intersection, SplineC _spline, float _distFromCorner, bool _isOverridingRegular = false)
        {
            Vector3 POS = default(Vector3);
            if (!_intersection.isRegularPoleAlignment && !_isOverridingRegular)
            {
                //float tDist = ((Vector3.Distance(GSDRI.CornerRR,GSDRI.CornerRL) / 2f) + DistFromCorner) / tSpline.distance;;
                float p = Mathf.Clamp(_intersection.node1.time, 0f, 1f);
                POS = _spline.GetSplineValue(p, true);
                POS = Vector3.Cross(POS, Vector3.up);
                return POS;
            }
            else
            {
                POS = _intersection.cornerRL - _intersection.cornerLL;
                return POS * -1;
            }
        }


        private static Vector3 TrafficLightBaseGetRotLR(RoadIntersection _intersection, SplineC _spline, float _distFromCorner, bool _isOverridingRegular = false)
        {
            Vector3 POS = default(Vector3);
            if (!_intersection.isRegularPoleAlignment && !_isOverridingRegular)
            {
                //				float tDist = ((Vector3.Distance(GSDRI.CornerLR,GSDRI.CornerLL) / 2f) + DistFromCorner) / tSpline.distance;;
                float p = Mathf.Clamp(_intersection.node1.time, 0f, 1f);
                POS = _spline.GetSplineValue(p, true);
                POS = Vector3.Cross(POS, Vector3.up);
                return POS * -1;
            }
            else
            {
                POS = _intersection.cornerRR - _intersection.cornerLR;
                return POS;
            }
        }


        private static Vector3 TrafficLightBaseGetRotRR(RoadIntersection _intersection, SplineC _spline, float _distFromCorner, bool _isOverridingRegular = false)
        {
            Vector3 POS = default(Vector3);
            if (!_intersection.isRegularPoleAlignment && !_isOverridingRegular)
            {
                //				float tDist = ((Vector3.Distance(GSDRI.CornerRR,GSDRI.CornerLR) / 2f) + DistFromCorner) / tSpline.distance;;
                float p = Mathf.Clamp(_intersection.node2.time, 0f, 1f);
                POS = _spline.GetSplineValue(p, true);
                POS = Vector3.Cross(POS, Vector3.up);
                if (_intersection.isFlipped)
                {
                    POS = POS * -1;
                }
            }
            else
            {
                POS = _intersection.cornerLL - _intersection.cornerLR;
            }
            return POS;
        }


        private static Vector3 TrafficLightBaseGetRotLL(RoadIntersection _intersection, SplineC _spline, float _distFromCorner, bool _isOverridingRegular = false)
        {
            Vector3 POS = default(Vector3);
            if (!_intersection.isRegularPoleAlignment && !_isOverridingRegular)
            {
                //				float tDist = ((Vector3.Distance(GSDRI.CornerLL,GSDRI.CornerRL) / 2f) + DistFromCorner) / tSpline.distance;;
                float p = Mathf.Clamp(_intersection.node2.time, 0f, 1f);
                POS = _spline.GetSplineValue(p, true);
                POS = Vector3.Cross(POS, Vector3.up);
                if (_intersection.isFlipped)
                {
                    POS = POS * -1;
                }
            }
            else
            {
                POS = _intersection.cornerRL - _intersection.cornerRR;
            }
            return POS * -1;
        }
        #endregion


        #region "Traffic light mains"
        private static void CreateTrafficLightMains(GameObject _masterGameObj, GameObject _RR, GameObject _RL, GameObject _LL, GameObject _LR)
        {
            RoadIntersection GSDRI = _masterGameObj.GetComponent<RoadIntersection>();
            SplineC tSpline = GSDRI.node1.spline;

            float tDist = (Vector3.Distance(GSDRI.cornerRL, GSDRI.cornerRR) / 2f) / tSpline.distance;
            Vector3 tan = tSpline.GetSplineValue(GSDRI.node1.time + tDist, true);
            ProcessPole(_masterGameObj, _RL, tan * -1, 1, Vector3.Distance(GSDRI.cornerRL, GSDRI.cornerRR));
            tDist = (Vector3.Distance(GSDRI.cornerLR, GSDRI.cornerLL) / 2f) / tSpline.distance;
            tan = tSpline.GetSplineValue(GSDRI.node1.time - tDist, true);
            ProcessPole(_masterGameObj, _LR, tan, 3, Vector3.Distance(GSDRI.cornerLR, GSDRI.cornerLL));


            float InterDist = Vector3.Distance(GSDRI.cornerRL, GSDRI.cornerLL);
            tDist = (InterDist / 2f) / tSpline.distance;
            tan = tSpline.GetSplineValue(GSDRI.node1.time + tDist, true);

            float fTime1 = GSDRI.node2.time + tDist;
            float fTime2neg = GSDRI.node2.time - tDist;

            tSpline = GSDRI.node2.spline;
            if (GSDRI.isFlipped)
            {
                tan = tSpline.GetSplineValue(fTime1, true);
                ProcessPole(_masterGameObj, _RR, tan, 0, InterDist);
                tan = tSpline.GetSplineValue(fTime2neg, true);
                ProcessPole(_masterGameObj, _LL, tan * -1, 2, InterDist);
            }
            else
            {
                tan = tSpline.GetSplineValue(fTime2neg, true);
                ProcessPole(_masterGameObj, _RR, tan * -1, 0, InterDist);
                tan = tSpline.GetSplineValue(fTime1, true);
                ProcessPole(_masterGameObj, _LL, tan, 2, InterDist);
            }

            if (GSDRI.ignoreCorner == 0)
            {
                if (_RR != null)
                {
                    Object.DestroyImmediate(_RR);
                }
            }
            else if (GSDRI.ignoreCorner == 1)
            {
                if (_RL != null)
                {
                    Object.DestroyImmediate(_RL);
                }
            }
            else if (GSDRI.ignoreCorner == 2)
            {
                if (_LL != null)
                {
                    Object.DestroyImmediate(_LL);
                }
            }
            else if (GSDRI.ignoreCorner == 3)
            {
                if (_LR != null)
                {
                    Object.DestroyImmediate(_LR);
                }
            }
        }


        private static void AdjustLightPrefab(GameObject _light)
        {
            string basePath = RoadEditorUtility.GetBasePath();

            foreach (Light light in _light.GetComponentsInChildren<Light>())
            {
                if (light.type == LightType.Point)
                {
                    light.flare = UnityEditor.AssetDatabase.LoadAssetAtPath<Flare>(basePath + "/Flares/GSDSodiumBulb.flare");
                }
            }
        }


        private static void ProcessPole(GameObject _masterGameObj, GameObject _obj, Vector3 _tan, int _corner, float _interDist)
        {
            RoadIntersection intersection = _masterGameObj.GetComponent<RoadIntersection>();
            SplineC spline = intersection.node1.spline;
            //			bool bIsRB = true;

            //			float RoadWidth = tSpline.tRoad.RoadWidth();
            float LaneWidth = spline.road.laneWidth;
            float ShoulderWidth = spline.road.shoulderWidth;

            int Lanes = spline.road.laneAmount;
            int LanesHalf = Lanes / 2;
            float LanesForInter = -1;
            if (intersection.roadType == RoadIntersection.RoadTypeEnum.BothTurnLanes)
            {
                LanesForInter = LanesHalf + 1f;
            }
            else if (intersection.roadType == RoadIntersection.RoadTypeEnum.TurnLane)
            {
                LanesForInter = LanesHalf + 1f;
            }
            else
            {
                LanesForInter = LanesHalf;
            }
            float DistFromCorner = (ShoulderWidth * 0.35f);
            float TLDistance = (LanesForInter * LaneWidth) + DistFromCorner;

            MeshFilter MF = _obj.GetComponent<MeshFilter>();
            Mesh tMesh = MF.sharedMesh;
            Vector3[] tVerts = tMesh.vertices;
            Vector3 StartVec = tVerts[520];
            Vector3 EndVec = tVerts[521];
            StartVec = (((EndVec - StartVec) * (DistFromCorner / TLDistance)) + StartVec);
            Vector3 tempR_Start = tVerts[399];
            Vector3 tempR_End = tVerts[396];
            Vector3 tLanePosR = ((tempR_End - tempR_Start) * 0.95f) + tempR_Start;
            tLanePosR.z -= 1f;

            float SmallerDist = Vector3.Distance(StartVec, EndVec);

            //StartVec = Corner
            //2.5m = lane
            //7.5m = lane
            //12.5m = lane
            Vector3[] tLanePos = new Vector3[LanesHalf];
            for (int index = 0; index < LanesHalf; index++)
            {
                tLanePos[index] = (((EndVec - StartVec) * (((LaneWidth * 0.5f) + (index * LaneWidth)) / SmallerDist)) + StartVec);
            }
            Vector3 tLanePosL = default(Vector3);
            Vector3 tLanePosL_Sign = default(Vector3);

            if (intersection.isLeftTurnYieldOnGreen)
            {
                tLanePosL = ((EndVec - StartVec) * ((SmallerDist - 1.8f) / SmallerDist)) + StartVec;
                tLanePosL_Sign = ((EndVec - StartVec) * ((SmallerDist - 3.2f) / SmallerDist)) + StartVec;
            }
            else
            {
                tLanePosL = ((EndVec - StartVec) * ((SmallerDist - 2.5f) / SmallerDist)) + StartVec;
            }

            Vector3 tPos1 = default(Vector3);
            if (_corner == 0)
            { //RR
                tPos1 = intersection.cornerLR;
            }
            else if (_corner == 1)
            { //RL
                tPos1 = intersection.cornerRR;
            }
            else if (_corner == 2)
            { //LL
                tPos1 = intersection.cornerRL;
            }
            else if (_corner == 3)
            { //LR
                tPos1 = intersection.cornerLL;
            }

            int mCount = tLanePos.Length;
            float mDistance = -50000f;
            float tDistance = 0f;
            for (int index = 0; index < mCount; index++)
            {
                tDistance = Vector3.Distance(_obj.transform.TransformPoint(tLanePos[index]), tPos1);
                if (tDistance > mDistance)
                {
                    mDistance = tDistance;
                }
            }
            tDistance = Vector3.Distance(_obj.transform.TransformPoint(tLanePosL), tPos1);
            if (tDistance > mDistance)
            {
                mDistance = tDistance;
            }
            tDistance = Vector3.Distance(_obj.transform.TransformPoint(tLanePosR), tPos1);
            if (tDistance > mDistance)
            {
                mDistance = tDistance;
            }

            float tScaleSense = ((200f - intersection.scalingSense) / 200f) * 200f;
            tScaleSense = Mathf.Clamp(tScaleSense * 0.1f, 0f, 20f);
            float ScaleMod = ((mDistance / 17f) + tScaleSense) * (1f / (tScaleSense + 1f));
            if (RootUtils.IsApproximately(tScaleSense, 20f, 0.05f))
            {
                ScaleMod = 1f;
            }
            ScaleMod = Mathf.Clamp(ScaleMod, 1f, 1.5f);
            Vector3 tScale = new Vector3(ScaleMod, ScaleMod, ScaleMod);
            bool bScale = true;
            if (RootUtils.IsApproximately(ScaleMod, 1f, 0.001f))
            {
                bScale = false;
            }

            //Debug.Log (mDistance + " " + ScaleMod + " " + tScaleSense);

            GameObject tRight = null;
            GameObject tLeft = null;
            GameObject tLeft_Sign = null;
            Object prefab = null;

            MeshRenderer MR_Left = null;
            MeshRenderer MR_Right = null;
            MeshRenderer[] MR_Mains = new MeshRenderer[LanesHalf];
            int cCount = -1;

            string basePath = RoadEditorUtility.GetBasePath();

            if (intersection.roadType != RoadIntersection.RoadTypeEnum.NoTurnLane)
            {
                if (intersection.isLeftTurnYieldOnGreen)
                {
                    prefab = UnityEditor.AssetDatabase.LoadAssetAtPath(basePath + "/Mesh/RoadObj/Signs/GSDTrafficLightLeftYield.prefab", typeof(GameObject));
                }
                else
                {
                    prefab = UnityEditor.AssetDatabase.LoadAssetAtPath(basePath + "/Mesh/RoadObj/Signs/GSDTrafficLightLeft.prefab", typeof(GameObject));
                }
                tLeft = (GameObject)GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
                AdjustLightPrefab(tLeft);
                tLeft.transform.position = _obj.transform.TransformPoint(tLanePosL);
                tLeft.transform.rotation = Quaternion.LookRotation(_tan) * Quaternion.Euler(0f, 90f, 0f);
                tLeft.transform.parent = _obj.transform;
                tLeft.transform.name = "LightLeft";

                cCount = tLeft.transform.childCount;
                for (int index = 0; index < cCount; index++)
                {
                    if (tLeft.transform.GetChild(index).name.ToLower() == "lights")
                    {
                        MR_Left = tLeft.transform.GetChild(index).GetComponent<MeshRenderer>();
                    }
                }

                if (bScale)
                {
                    tLeft.transform.localScale = tScale;
                }

                if (intersection.isLeftTurnYieldOnGreen)
                {
                    prefab = UnityEditor.AssetDatabase.LoadAssetAtPath(basePath + "/Mesh/RoadObj/Signs/GSDSignYieldOnGreen.prefab", typeof(GameObject));
                    tLeft_Sign = (GameObject)GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
                    tLeft_Sign.transform.position = _obj.transform.TransformPoint(tLanePosL_Sign);
                    tLeft_Sign.transform.rotation = Quaternion.LookRotation(_tan) * Quaternion.Euler(-90f, 90f, 0f);
                    tLeft_Sign.transform.parent = _obj.transform;
                    tLeft_Sign.transform.name = "SignYieldOnGreen";
                    if (bScale)
                    {
                        tLeft_Sign.transform.localScale = tScale;
                    }
                }
            }
            if (intersection.roadType == RoadIntersection.RoadTypeEnum.BothTurnLanes)
            {
                prefab = UnityEditor.AssetDatabase.LoadAssetAtPath(basePath + "/Mesh/RoadObj/Signs/GSDTrafficLightRight.prefab", typeof(GameObject));
                tRight = (GameObject)GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
                AdjustLightPrefab(tRight);
                tRight.transform.position = _obj.transform.TransformPoint(tLanePosR);
                tRight.transform.rotation = Quaternion.LookRotation(_tan) * Quaternion.Euler(0f, 90f, 0f);
                tRight.transform.parent = _obj.transform;
                tRight.transform.name = "LightRight";
                if (bScale)
                {
                    tRight.transform.localScale = tScale;
                }

                cCount = tRight.transform.childCount;
                for (int index = 0; index < cCount; index++)
                {
                    if (tRight.transform.GetChild(index).name.ToLower() == "lights")
                    {
                        MR_Right = tRight.transform.GetChild(index).GetComponent<MeshRenderer>();
                    }
                }
            }
            GameObject[] tLanes = new GameObject[LanesHalf];
            for (int index = 0; index < LanesHalf; index++)
            {
                prefab = UnityEditor.AssetDatabase.LoadAssetAtPath(basePath + "/Mesh/RoadObj/Signs/GSDTrafficLightMain.prefab", typeof(GameObject));
                tLanes[index] = (GameObject)GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
                AdjustLightPrefab(tLanes[index]);
                tLanes[index].transform.position = _obj.transform.TransformPoint(tLanePos[index]);
                tLanes[index].transform.rotation = Quaternion.LookRotation(_tan) * Quaternion.Euler(0f, 90f, 0f);
                tLanes[index].transform.parent = _obj.transform;
                tLanes[index].transform.name = "Light" + index.ToString();
                if (bScale)
                {
                    tLanes[index].transform.localScale = tScale;
                }

                cCount = tLanes[index].transform.childCount;
                for (int j = 0; j < cCount; j++)
                {
                    if (tLanes[index].transform.GetChild(j).name.ToLower() == "lights")
                    {
                        MR_Mains[index] = tLanes[index].transform.GetChild(j).GetComponent<MeshRenderer>();
                    }
                }
            }

            TrafficLightController LM = new TrafficLightController(ref tLeft, ref tRight, ref tLanes, ref MR_Left, ref MR_Right, ref MR_Mains);
            if (_corner == 0)
            {
                intersection.lightsRR = null;
                intersection.lightsRR = LM;
            }
            else if (_corner == 1)
            {
                intersection.lightsRL = null;
                intersection.lightsRL = LM;
            }
            else if (_corner == 2)
            {
                intersection.lightsLL = null;
                intersection.lightsLL = LM;
            }
            else if (_corner == 3)
            {
                intersection.lightsLR = null;
                intersection.lightsLR = LM;
            }
        }
        #endregion


        public static void GetFourPoints(RoadIntersection _roadIntersection, out Vector3 _posRR, out Vector3 _posRL, out Vector3 _posLL, out Vector3 _posLR, float _distFromCorner)
        {
            GetFourPointsDo(ref _roadIntersection, out _posRR, out _posRL, out _posLL, out _posLR, _distFromCorner);
        }


        private static void GetFourPointsDo(ref RoadIntersection _roadIntersection, out Vector3 _posRR, out Vector3 _posRL, out Vector3 _posLL, out Vector3 _posLR, float _distFromCorner)
        {
            //Get four points:
            float tPos1 = 0f;
            float tPos2 = 0f;
            Vector3 tTan1 = default(Vector3);
            Vector3 tTan2 = default(Vector3);
            float Node1Width = -1f;
            float Node2Width = -1f;
            Vector3 tVectRR = _roadIntersection.cornerRR;
            Vector3 tVectRL = _roadIntersection.cornerRL;
            Vector3 tVectLR = _roadIntersection.cornerLR;
            Vector3 tVectLL = _roadIntersection.cornerLL;
            Vector3 tDir = default(Vector3);
            float ShoulderWidth1 = _roadIntersection.node1.spline.road.shoulderWidth;
            float ShoulderWidth2 = _roadIntersection.node2.spline.road.shoulderWidth;

            if (!_roadIntersection.isFlipped)
            {
                //RR:
                Node1Width = (Vector3.Distance(_roadIntersection.cornerRR, _roadIntersection.node1.pos) + ShoulderWidth1) / _roadIntersection.node1.spline.distance;
                Node2Width = (Vector3.Distance(_roadIntersection.cornerRR, _roadIntersection.node2.pos) + ShoulderWidth2) / _roadIntersection.node2.spline.distance;
                tPos1 = _roadIntersection.node1.time - Node1Width;
                tTan1 = _roadIntersection.node1.spline.GetSplineValue(tPos1, true) * -1f;
                tPos2 = _roadIntersection.node2.time + Node2Width;
                tTan2 = _roadIntersection.node2.spline.GetSplineValue(tPos2, true);
                tDir = (tTan1.normalized + tTan2.normalized).normalized;
                _posRR = tVectRR + (tDir * _distFromCorner);
                //RL:
                Node1Width = (Vector3.Distance(_roadIntersection.cornerRL, _roadIntersection.node1.pos) + ShoulderWidth1) / _roadIntersection.node1.spline.distance;
                Node2Width = (Vector3.Distance(_roadIntersection.cornerRL, _roadIntersection.node2.pos) + ShoulderWidth2) / _roadIntersection.node2.spline.distance;
                tPos1 = _roadIntersection.node1.time + Node1Width;
                if (_roadIntersection.intersectionType == RoadIntersection.IntersectionTypeEnum.ThreeWay)
                {
                    tPos1 = _roadIntersection.node1.time;
                }
                tTan1 = _roadIntersection.node1.spline.GetSplineValue(tPos1, true);
                tPos2 = _roadIntersection.node2.time + Node2Width;
                tTan2 = _roadIntersection.node2.spline.GetSplineValue(tPos2, true);
                tDir = (tTan1.normalized + tTan2.normalized).normalized;
                _posRL = tVectRL + (tDir * _distFromCorner);
                //LL:
                Node1Width = (Vector3.Distance(_roadIntersection.cornerLL, _roadIntersection.node1.pos) + ShoulderWidth1) / _roadIntersection.node1.spline.distance;
                Node2Width = (Vector3.Distance(_roadIntersection.cornerLL, _roadIntersection.node2.pos) + ShoulderWidth2) / _roadIntersection.node2.spline.distance;
                tPos1 = _roadIntersection.node1.time + Node1Width;
                if (_roadIntersection.intersectionType == RoadIntersection.IntersectionTypeEnum.ThreeWay)
                {
                    tPos1 = _roadIntersection.node1.time;
                }
                tTan1 = _roadIntersection.node1.spline.GetSplineValue(tPos1, true);
                tPos2 = _roadIntersection.node2.time - Node2Width;
                tTan2 = _roadIntersection.node2.spline.GetSplineValue(tPos2, true) * -1f;
                tDir = (tTan1.normalized + tTan2.normalized).normalized;
                _posLL = tVectLL + (tDir * _distFromCorner);
                //LR:
                Node1Width = (Vector3.Distance(_roadIntersection.cornerLR, _roadIntersection.node1.pos) + ShoulderWidth1) / _roadIntersection.node1.spline.distance;
                Node2Width = (Vector3.Distance(_roadIntersection.cornerLR, _roadIntersection.node2.pos) + ShoulderWidth2) / _roadIntersection.node2.spline.distance;
                tPos1 = _roadIntersection.node1.time - Node1Width;
                tTan1 = _roadIntersection.node1.spline.GetSplineValue(tPos1, true) * -1f;
                tPos2 = _roadIntersection.node2.time - Node2Width;
                tTan2 = _roadIntersection.node2.spline.GetSplineValue(tPos2, true) * -1f;
                tDir = (tTan1.normalized + tTan2.normalized).normalized;
                _posLR = tVectLR + (tDir * _distFromCorner);
            }
            else
            {
                //RR:
                Node1Width = (Vector3.Distance(_roadIntersection.cornerRR, _roadIntersection.node1.pos) + ShoulderWidth1) / _roadIntersection.node1.spline.distance;
                Node2Width = (Vector3.Distance(_roadIntersection.cornerRR, _roadIntersection.node2.pos) + ShoulderWidth2) / _roadIntersection.node2.spline.distance;
                tPos1 = _roadIntersection.node1.time - Node1Width;
                tTan1 = _roadIntersection.node1.spline.GetSplineValue(tPos1, true) * -1f;
                tPos2 = _roadIntersection.node2.time - Node2Width;
                tTan2 = _roadIntersection.node2.spline.GetSplineValue(tPos2, true) * -1f;
                tDir = (tTan1.normalized + tTan2.normalized).normalized;
                _posRR = tVectRR + (tDir * _distFromCorner);
                //RL:
                Node1Width = (Vector3.Distance(_roadIntersection.cornerRL, _roadIntersection.node1.pos) + ShoulderWidth1) / _roadIntersection.node1.spline.distance;
                Node2Width = (Vector3.Distance(_roadIntersection.cornerRL, _roadIntersection.node2.pos) + ShoulderWidth2) / _roadIntersection.node2.spline.distance;
                tPos1 = _roadIntersection.node1.time + Node1Width;
                if (_roadIntersection.intersectionType == RoadIntersection.IntersectionTypeEnum.ThreeWay)
                {
                    tPos1 = _roadIntersection.node1.time;
                }
                tTan1 = _roadIntersection.node1.spline.GetSplineValue(tPos1, true);
                tPos2 = _roadIntersection.node2.time - Node2Width;
                tTan2 = _roadIntersection.node2.spline.GetSplineValue(tPos2, true) * -1f;
                tDir = (tTan1.normalized + tTan2.normalized).normalized;
                _posRL = tVectRL + (tDir * _distFromCorner);
                //LL:
                Node1Width = (Vector3.Distance(_roadIntersection.cornerLL, _roadIntersection.node1.pos) + ShoulderWidth1) / _roadIntersection.node1.spline.distance;
                Node2Width = (Vector3.Distance(_roadIntersection.cornerLL, _roadIntersection.node2.pos) + ShoulderWidth2) / _roadIntersection.node2.spline.distance;
                tPos1 = _roadIntersection.node1.time + Node1Width;
                if (_roadIntersection.intersectionType == RoadIntersection.IntersectionTypeEnum.ThreeWay)
                {
                    tPos1 = _roadIntersection.node1.time;
                }
                tTan1 = _roadIntersection.node1.spline.GetSplineValue(tPos1, true);
                tPos2 = _roadIntersection.node2.time + Node2Width;
                tTan2 = _roadIntersection.node2.spline.GetSplineValue(tPos2, true);
                tDir = (tTan1.normalized + tTan2.normalized).normalized;
                _posLL = tVectLL + (tDir * _distFromCorner);
                //LR:
                Node1Width = (Vector3.Distance(_roadIntersection.cornerLR, _roadIntersection.node1.pos) + ShoulderWidth1) / _roadIntersection.node1.spline.distance;
                Node2Width = (Vector3.Distance(_roadIntersection.cornerLR, _roadIntersection.node2.pos) + ShoulderWidth2) / _roadIntersection.node2.spline.distance;
                tPos1 = _roadIntersection.node1.time - Node1Width;
                tTan1 = _roadIntersection.node1.spline.GetSplineValue(tPos1, true) * -1f;
                tPos2 = _roadIntersection.node2.time + Node2Width;
                tTan2 = _roadIntersection.node2.spline.GetSplineValue(tPos2, true);
                tDir = (tTan1.normalized + tTan2.normalized).normalized;
                _posLR = tVectLR + (tDir * _distFromCorner);
            }
            _posRR.y = _roadIntersection.signHeight;
            _posRL.y = _roadIntersection.signHeight;
            _posLL.y = _roadIntersection.signHeight;
            _posLR.y = _roadIntersection.signHeight;

            //			GameObject tObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //			tObj.transform.localScale = new Vector3(0.2f,20f,0.2f);
            //			tObj.transform.name = "temp22_RR";
            //			tObj.transform.position = tPosRR;
            //			tObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //			tObj.transform.localScale = new Vector3(0.2f,20f,0.2f);
            //			tObj.transform.name = "temp22_RL";
            //			tObj.transform.position = tPosRL;
            //			tObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //			tObj.transform.localScale = new Vector3(0.2f,20f,0.2f);
            //			tObj.transform.name = "temp22_LL";
            //			tObj.transform.position = tPosLL;
            //			tObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //			tObj.transform.localScale = new Vector3(0.2f,20f,0.2f);
            //			tObj.transform.name = "temp22_LR";
            //			tObj.transform.position = tPosLR;
        }


        /*
        //		public static void GetOrigFour(GSDRoadIntersection GSDRI, out Vector3 tPosRR, out Vector3 tPosRL, out Vector3 tPosLL, out Vector3 tPosLR){
        //			//Get four points:
        //			float tPos1 = 0f;
        //			float tPos2 = 0f;
        //			Vector3 tTan1 = default(Vector3);
        //			Vector3 tTan2 = default(Vector3);
        //			float Node1Width = -1f;
        //			float Node2Width = -1f;
        //			Vector3 tDirRR = default(Vector3);
        //			Vector3 tDirRL = default(Vector3);
        //			Vector3 tDirLL = default(Vector3);
        //			Vector3 tDirLR = default(Vector3);
        //			float tAngleRR = 85f;
        //			float tAngleRL = 85f;
        //			float tAngleLL = 85f;
        //			float tAngleLR = 85f;
        //			float ShoulderWidth1 = GSDRI.Node1.GSDSpline.tRoad.opt_ShoulderWidth;
        //			float ShoulderWidth2 = GSDRI.Node2.GSDSpline.tRoad.opt_ShoulderWidth;
        //			Vector3 xPos1 = default(Vector3);
        //			Vector3 xPos2 = default(Vector3);
        //			
        //			if(!GSDRI.bFlipped){
        //				//RR:
        //				Node1Width = (Vector3.Distance(GSDRI.CornerRR,GSDRI.Node1.pos) + ShoulderWidth1)/GSDRI.Node1.GSDSpline.distance;
        //				Node2Width = (Vector3.Distance(GSDRI.CornerRR,GSDRI.Node2.pos) + ShoulderWidth2)/GSDRI.Node2.GSDSpline.distance;
        //				tPos1 = GSDRI.Node1.tTime - Node1Width;
        //				tTan1 = GSDRI.Node1.GSDSpline.GetSplineValue(tPos1,true) * -1f;
        //				tPos2 = GSDRI.Node2.tTime + Node2Width;
        //				tTan2 = GSDRI.Node2.GSDSpline.GetSplineValue(tPos2,true);
        //				xPos1 = GSDRI.Node1.GSDSpline.GetSplineValue(tPos1);
        //				xPos2 = GSDRI.Node1.GSDSpline.GetSplineValue(tPos2);
        //				tDirRR = (tTan1.normalized + tTan2.normalized).normalized;
        //				//tAngleRR = Vector3.Angle(tTan1,tTan2);
        //				tAngleRR = Vector3.Angle(xPos1 - GSDRI.Node1.pos,xPos2 - GSDRI.Node1.pos);
        //				//RL:
        //				Node1Width = (Vector3.Distance(GSDRI.CornerRL,GSDRI.Node1.pos) + ShoulderWidth1)/GSDRI.Node1.GSDSpline.distance;
        //				Node2Width = (Vector3.Distance(GSDRI.CornerRL,GSDRI.Node2.pos) + ShoulderWidth2)/GSDRI.Node2.GSDSpline.distance;
        //				tPos1 = GSDRI.Node1.tTime + Node1Width;
        //				tTan1 = GSDRI.Node1.GSDSpline.GetSplineValue(tPos1,true);
        //				tPos2 = GSDRI.Node2.tTime + Node2Width;
        //				tTan2 = GSDRI.Node2.GSDSpline.GetSplineValue(tPos2,true);
        //				xPos1 = GSDRI.Node1.GSDSpline.GetSplineValue(tPos1);
        //				xPos2 = GSDRI.Node1.GSDSpline.GetSplineValue(tPos2);
        //				tDirRL = (tTan1.normalized + tTan2.normalized).normalized;
        //				//tAngleRL = Vector3.Angle(tTan1,tTan2);
        //				tAngleRL = Vector3.Angle(xPos1 - GSDRI.Node1.pos,xPos2 - GSDRI.Node1.pos);
        //				//LL:
        //				Node1Width = (Vector3.Distance(GSDRI.CornerLL,GSDRI.Node1.pos) + ShoulderWidth1)/GSDRI.Node1.GSDSpline.distance;
        //				Node2Width = (Vector3.Distance(GSDRI.CornerLL,GSDRI.Node2.pos) + ShoulderWidth2)/GSDRI.Node2.GSDSpline.distance;
        //				tPos1 = GSDRI.Node1.tTime + Node1Width;
        //				tTan1 = GSDRI.Node1.GSDSpline.GetSplineValue(tPos1,true);
        //				tPos2 = GSDRI.Node2.tTime - Node2Width;
        //				tTan2 = GSDRI.Node2.GSDSpline.GetSplineValue(tPos2,true) * -1f;
        //				xPos1 = GSDRI.Node1.GSDSpline.GetSplineValue(tPos1);
        //				xPos2 = GSDRI.Node1.GSDSpline.GetSplineValue(tPos2);
        //				tDirLL = (tTan1.normalized + tTan2.normalized).normalized;
        //				//tAngleLL = Vector3.Angle(tTan1,tTan2);
        //				tAngleLL = Vector3.Angle(xPos1 - GSDRI.Node1.pos,xPos2 - GSDRI.Node1.pos);
        //				//LR:
        //				Node1Width = (Vector3.Distance(GSDRI.CornerLR,GSDRI.Node1.pos) + ShoulderWidth1)/GSDRI.Node1.GSDSpline.distance;
        //				Node2Width = (Vector3.Distance(GSDRI.CornerLR,GSDRI.Node2.pos) + ShoulderWidth2)/GSDRI.Node2.GSDSpline.distance;
        //				tPos1 = GSDRI.Node1.tTime - Node1Width;
        //				tTan1 = GSDRI.Node1.GSDSpline.GetSplineValue(tPos1,true) * -1f;
        //				tPos2 = GSDRI.Node2.tTime - Node2Width;
        //				tTan2 = GSDRI.Node2.GSDSpline.GetSplineValue(tPos2,true) * -1f;
        //				xPos1 = GSDRI.Node1.GSDSpline.GetSplineValue(tPos1);
        //				xPos2 = GSDRI.Node1.GSDSpline.GetSplineValue(tPos2);
        //				tDirLR = (tTan1.normalized + tTan2.normalized).normalized;
        //				//tAngleLR = Vector3.Angle(tTan1,tTan2);
        //				tAngleLR = Vector3.Angle(xPos1 - GSDRI.Node1.pos,xPos2 - GSDRI.Node1.pos);
        //			}else{
        //				//RR:
        //				Node1Width = (Vector3.Distance(GSDRI.CornerRR,GSDRI.Node1.pos) + ShoulderWidth1)/GSDRI.Node1.GSDSpline.distance;
        //				Node2Width = (Vector3.Distance(GSDRI.CornerRR,GSDRI.Node2.pos) + ShoulderWidth2)/GSDRI.Node2.GSDSpline.distance;
        //				tPos1 = GSDRI.Node1.tTime - Node1Width;
        //				tTan1 = GSDRI.Node1.GSDSpline.GetSplineValue(tPos1,true) * -1f;
        //				tPos2 = GSDRI.Node2.tTime - Node2Width;
        //				tTan2 = GSDRI.Node2.GSDSpline.GetSplineValue(tPos2,true) * -1f;
        //				tDirRR = (tTan1.normalized + tTan2.normalized).normalized;
        ////				tAngleRR = Vector3.Angle(tTan1,tTan2);
        //				xPos1 = GSDRI.Node1.GSDSpline.GetSplineValue(tPos1);
        //				xPos2 = GSDRI.Node1.GSDSpline.GetSplineValue(tPos2);
        //				tAngleRR = Vector3.Angle(xPos1 - GSDRI.Node1.pos,xPos2 - GSDRI.Node1.pos);
        //				//RL:
        //				Node1Width = (Vector3.Distance(GSDRI.CornerRL,GSDRI.Node1.pos) + ShoulderWidth1)/GSDRI.Node1.GSDSpline.distance;
        //				Node2Width = (Vector3.Distance(GSDRI.CornerRL,GSDRI.Node2.pos) + ShoulderWidth2)/GSDRI.Node2.GSDSpline.distance;
        //				tPos1 = GSDRI.Node1.tTime + Node1Width;
        //				tTan1 = GSDRI.Node1.GSDSpline.GetSplineValue(tPos1,true);
        //				tPos2 = GSDRI.Node2.tTime - Node2Width;
        //				tTan2 = GSDRI.Node2.GSDSpline.GetSplineValue(tPos2,true) * -1f;
        //				tDirRL = (tTan1.normalized + tTan2.normalized).normalized;
        ////				tAngleRL = Vector3.Angle(tTan1,tTan2);
        //				xPos1 = GSDRI.Node1.GSDSpline.GetSplineValue(tPos1);
        //				xPos2 = GSDRI.Node1.GSDSpline.GetSplineValue(tPos2);
        //				tAngleRL = Vector3.Angle(xPos1 - GSDRI.Node1.pos,xPos2 - GSDRI.Node1.pos);
        //				//LL:
        //				Node1Width = (Vector3.Distance(GSDRI.CornerLL,GSDRI.Node1.pos) + ShoulderWidth1)/GSDRI.Node1.GSDSpline.distance;
        //				Node2Width = (Vector3.Distance(GSDRI.CornerLL,GSDRI.Node2.pos) + ShoulderWidth2)/GSDRI.Node2.GSDSpline.distance;
        //				tPos1 = GSDRI.Node1.tTime + Node1Width;
        //				tTan1 = GSDRI.Node1.GSDSpline.GetSplineValue(tPos1,true);
        //				tPos2 = GSDRI.Node2.tTime + Node2Width;
        //				tTan2 = GSDRI.Node2.GSDSpline.GetSplineValue(tPos2,true);
        //				tDirLL = (tTan1.normalized + tTan2.normalized).normalized;
        ////				tAngleLL = Vector3.Angle(tTan1,tTan2);
        //				xPos1 = GSDRI.Node1.GSDSpline.GetSplineValue(tPos1);
        //				xPos2 = GSDRI.Node1.GSDSpline.GetSplineValue(tPos2);
        //				tAngleLL = Vector3.Angle(xPos1 - GSDRI.Node1.pos,xPos2 - GSDRI.Node1.pos);
        //				//LR:
        //				Node1Width = (Vector3.Distance(GSDRI.CornerLR,GSDRI.Node1.pos) + ShoulderWidth1)/GSDRI.Node1.GSDSpline.distance;
        //				Node2Width = (Vector3.Distance(GSDRI.CornerLR,GSDRI.Node2.pos) + ShoulderWidth2)/GSDRI.Node2.GSDSpline.distance;
        //				tPos1 = GSDRI.Node1.tTime - Node1Width;
        //				tTan1 = GSDRI.Node1.GSDSpline.GetSplineValue(tPos1,true) * -1f;
        //				tPos2 = GSDRI.Node2.tTime + Node2Width;
        //				tTan2 = GSDRI.Node2.GSDSpline.GetSplineValue(tPos2,true);
        //				tDirLR = (tTan1.normalized + tTan2.normalized).normalized;
        //				//tAngleLR = Vector3.Angle(tTan1,tTan2);
        //				xPos1 = GSDRI.Node1.GSDSpline.GetSplineValue(tPos1);
        //				xPos2 = GSDRI.Node1.GSDSpline.GetSplineValue(tPos2);
        //				tAngleLR = Vector3.Angle(xPos1 - GSDRI.Node1.pos,xPos2 - GSDRI.Node1.pos);
        //			}	
        //			
        //			//D = B*cos(angle/2)
        //			float tWidth = GSDRI.Node1.GSDSpline.tRoad.opt_RoadWidth * 0.5f;
        //			float tAngleRR_Opp = 180f - tAngleRR;
        //			float tAngleRL_Opp = 180f - tAngleRL;
        //			float tAngleLL_Opp = 180f - tAngleLL;
        //			float tAngleLR_Opp = 180f - tAngleLR;
        //
        //			float tOffSetRR = tWidth*(Mathf.Cos((tAngleRR*0.5f)*Mathf.Deg2Rad));
        //			float tOffSetRL = tWidth*(Mathf.Cos((tAngleRL*0.5f)*Mathf.Deg2Rad));
        //			float tOffSetLL = tWidth*(Mathf.Cos((tAngleLL*0.5f)*Mathf.Deg2Rad));
        //			float tOffSetLR = tWidth*(Mathf.Cos((tAngleLR*0.5f)*Mathf.Deg2Rad));
        //			
        //			float tOffSetRR_opp = tWidth*(Mathf.Cos((tAngleRR_Opp*0.5f)*Mathf.Deg2Rad));
        //			float tOffSetRL_opp = tWidth*(Mathf.Cos((tAngleRL_Opp*0.5f)*Mathf.Deg2Rad));
        //			float tOffSetLL_opp = tWidth*(Mathf.Cos((tAngleLL_Opp*0.5f)*Mathf.Deg2Rad));
        //			float tOffSetLR_opp = tWidth*(Mathf.Cos((tAngleLR_Opp*0.5f)*Mathf.Deg2Rad));
        //			
        //			Vector3 tPos = GSDRI.Node1.pos;
        //			
        ////			tOffSetRR *=2f;
        ////			tOffSetRL *=2f;
        ////			tOffSetLL *=2f;
        ////			tOffSetLR *=2f;
        //			
        //			tPosRR = tPos + (tDirRR * (tOffSetRR+tOffSetRR_opp));
        //			tPosRL = tPos + (tDirRL * (tOffSetRL+tOffSetRL_opp));
        //			tPosLL = tPos + (tDirLL * (tOffSetLL+tOffSetLL_opp));
        //			tPosLR = tPos + (tDirLR * (tOffSetLR+tOffSetLR_opp));
        //			
        //			GameObject tObj = GameObject.Find("temp22_RR"); if(tObj != null){ Object.DestroyImmediate(tObj); }
        //			tObj = GameObject.Find("temp22_RL"); if(tObj != null){ Object.DestroyImmediate(tObj); }
        //			tObj = GameObject.Find("temp22_LL"); if(tObj != null){ Object.DestroyImmediate(tObj); }
        //			tObj = GameObject.Find("temp22_LR"); if(tObj != null){ Object.DestroyImmediate(tObj); }
        //			
        //			GameObject tCubeRR = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //			tCubeRR.transform.position = tPosRR;
        //			tCubeRR.transform.name = "temp22_RR";
        //			tCubeRR.transform.localScale = new Vector3(0.2f,20f,0.2f);
        //			
        //			tCubeRR = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //			tCubeRR.transform.position = tPosRL;
        //			tCubeRR.transform.name = "temp22_RL";
        //			tCubeRR.transform.localScale = new Vector3(0.2f,20f,0.2f);
        //			
        //			tCubeRR = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //			tCubeRR.transform.position = tPosLL;
        //			tCubeRR.transform.name = "temp22_LL";
        //			tCubeRR.transform.localScale = new Vector3(0.2f,20f,0.2f);
        //			
        //			tCubeRR = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //			tCubeRR.transform.position = tPosLR;
        //			tCubeRR.transform.name = "temp22_LR";
        //			tCubeRR.transform.localScale = new Vector3(0.2f,20f,0.2f);
        //		}
        //	
        //		public static void GetCornerVectors_Test(GSDRoadIntersection GSDRI, out Vector3 tPosRR, out Vector3 tPosRL, out Vector3 tPosLL, out Vector3 tPosLR){
        //			GSDSplineN tNode = null;
        //			GSDSplineC tSpline = null;
        //
        //			tNode = GSDRI.Node1;
        //			tSpline = tNode.GSDSpline;
        //			float tOffset = tSpline.tRoad.opt_RoadWidth * 0.5f;
        //			float tPos1 = tNode.tTime - (tOffset/tSpline.distance);
        //			float tPos2 = tNode.tTime + (tOffset/tSpline.distance);
        //			Vector3 tVect1 = tSpline.GetSplineValue(tPos1);	
        //			Vector3 POS1 = tSpline.GetSplineValue(tPos1,true);
        //			Vector3 tVect2 = tSpline.GetSplineValue(tPos2);	
        //			Vector3 POS2 = tSpline.GetSplineValue(tPos2,true);
        //			tPosRR = (tVect1 + new Vector3(5f*POS1.normalized.z,0,5f*-POS1.normalized.x));
        //			tPosLR = (tVect1 + new Vector3(5f*-POS1.normalized.z,0,5f*POS1.normalized.x));
        //			tPosRL = (tVect2 + new Vector3(5f*POS2.normalized.z,0,5f*-POS2.normalized.x));
        //			tPosLL = (tVect2 + new Vector3(5f*-POS2.normalized.z,0,5f*POS2.normalized.x));
        //			
        //			tNode = GSDRI.Node2;
        //			tSpline = tNode.GSDSpline;
        //			tOffset = tSpline.tRoad.opt_RoadWidth * 0.5f;
        //			tPos1 = tNode.tTime - (tOffset/tSpline.distance);
        //			tPos2 = tNode.tTime + (tOffset/tSpline.distance);
        //			tVect1 = tSpline.GetSplineValue(tPos1);	
        //			POS1 = tSpline.GetSplineValue(tPos1,true);
        //			tVect2 = tSpline.GetSplineValue(tPos2);	
        //			POS2 = tSpline.GetSplineValue(tPos2,true);
        //			Vector3 tPosRR2 = default(Vector3);
        //			Vector3 tPosLR2 = default(Vector3);
        //			Vector3 tPosRL2 = default(Vector3);
        //			Vector3 tPosLL2 = default(Vector3);
        //			
        //			if(GSDRI.bFlipped){
        //				tPosRL2 = (tVect1 + new Vector3(5f*POS1.normalized.z,0,5f*-POS1.normalized.x));
        //				tPosRR2 = (tVect1 + new Vector3(5f*-POS1.normalized.z,0,5f*POS1.normalized.x));
        //				tPosLL2 = (tVect2 + new Vector3(5f*POS2.normalized.z,0,5f*-POS2.normalized.x));
        //				tPosLR2 = (tVect2 + new Vector3(5f*-POS2.normalized.z,0,5f*POS2.normalized.x));
        //			}else{
        //				tPosLR2 = (tVect1 + new Vector3(5f*POS1.normalized.z,0,5f*-POS1.normalized.x));
        //				tPosLL2 = (tVect1 + new Vector3(5f*-POS1.normalized.z,0,5f*POS1.normalized.x));
        //				tPosRR2 = (tVect2 + new Vector3(5f*POS2.normalized.z,0,5f*-POS2.normalized.x));
        //				tPosRL2 = (tVect2 + new Vector3(5f*-POS2.normalized.z,0,5f*POS2.normalized.x));
        //			}
        //			
        //			tPosRR = ((tPosRR-tPosRR2)*0.5f)+tPosRR;
        //			tPosLR = ((tPosLR-tPosLR2)*0.5f)+tPosLR;
        //			tPosRL = ((tPosRL-tPosRL2)*0.5f)+tPosRL;
        //			tPosLL = ((tPosLL-tPosLL2)*0.5f)+tPosLL;
        //			
        //			
        //						GameObject tObj = GameObject.Find("temp22_RR"); if(tObj != null){ Object.DestroyImmediate(tObj); }
        //			tObj = GameObject.Find("temp22_RL"); if(tObj != null){ Object.DestroyImmediate(tObj); }
        //			tObj = GameObject.Find("temp22_LL"); if(tObj != null){ Object.DestroyImmediate(tObj); }
        //			tObj = GameObject.Find("temp22_LR"); if(tObj != null){ Object.DestroyImmediate(tObj); }
        //			
        //			GameObject tCubeRR = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //			tCubeRR.transform.position = tPosRR;
        //			tCubeRR.transform.name = "temp22_RR";
        //			tCubeRR.transform.localScale = new Vector3(0.2f,20f,0.2f);
        //			
        //			tCubeRR = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //			tCubeRR.transform.position = tPosRL;
        //			tCubeRR.transform.name = "temp22_RL";
        //			tCubeRR.transform.localScale = new Vector3(0.2f,20f,0.2f);
        //			
        //			tCubeRR = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //			tCubeRR.transform.position = tPosLL;
        //			tCubeRR.transform.name = "temp22_LL";
        //			tCubeRR.transform.localScale = new Vector3(0.2f,20f,0.2f);
        //			
        //			tCubeRR = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //			tCubeRR.transform.position = tPosLR;
        //			tCubeRR.transform.name = "temp22_LR";
        //			tCubeRR.transform.localScale = new Vector3(0.2f,20f,0.2f);
        //		}
        */
    }
}