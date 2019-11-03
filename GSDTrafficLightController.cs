#region "Imports"
using UnityEngine;
#endregion


[System.Serializable]
public class GSDTrafficLightController
{
    //Enums for controller:
    public enum iLightControllerEnum { Regular, LeftTurn, MasterLeft1, MasterLeft2, Red }
    //Enums for actual lights:
    public enum iLightStatusEnum { Regular, LeftTurn, MasterLeft, Red, RightTurn }
    public enum iLightSubStatusEnum { Green, Yellow, Red }
    public enum iLightYieldSubStatusEnum { Green, Yellow, Red, YellowTurn, GreenTurn }


    [UnityEngine.Serialization.FormerlySerializedAs("LightLeftObj")]
    public GameObject lightLeftObject;
    [UnityEngine.Serialization.FormerlySerializedAs("LightRightObj")]
    public GameObject lightRightObject;
    [UnityEngine.Serialization.FormerlySerializedAs("LightsObj")]
    public GameObject[] lightsObjects;

    [UnityEngine.Serialization.FormerlySerializedAs("MR_Left")]
    public MeshRenderer leftMR;
    [UnityEngine.Serialization.FormerlySerializedAs("MR_Right")]
    public MeshRenderer rightMR;
    [UnityEngine.Serialization.FormerlySerializedAs("MR_MainsStorage")]
    public MeshRenderer[] mainMRStorage;
    [UnityEngine.Serialization.FormerlySerializedAs("MR_Main")]
    public MeshRenderer mainMR;

    [UnityEngine.Serialization.FormerlySerializedAs("LightLeft_R")]
    public Light lightLeftR;
    [UnityEngine.Serialization.FormerlySerializedAs("LightLeft_Y")]
    public Light lightLeftY;
    [UnityEngine.Serialization.FormerlySerializedAs("LightLeft_G")]
    public Light lightLeftG;

    [UnityEngine.Serialization.FormerlySerializedAs("LightRight_R")]
    public Light lightRightR;
    [UnityEngine.Serialization.FormerlySerializedAs("LightRight_Y")]
    public Light lightRightY;
    [UnityEngine.Serialization.FormerlySerializedAs("LightRight_G")]
    public Light lightRightG;

    [UnityEngine.Serialization.FormerlySerializedAs("Lights_R")]
    public Light[] lightsR;
    [UnityEngine.Serialization.FormerlySerializedAs("Lights_Y")]
    public Light[] lightsY;
    [UnityEngine.Serialization.FormerlySerializedAs("Lights_G")]
    public Light[] lightsG;

    [UnityEngine.Serialization.FormerlySerializedAs("iLightStatus")]
    public iLightStatusEnum lightStatus = iLightStatusEnum.Red;
    [UnityEngine.Serialization.FormerlySerializedAs("iLightSubStatus")]
    public iLightSubStatusEnum lightSubStatus = iLightSubStatusEnum.Green;

    [UnityEngine.Serialization.FormerlySerializedAs("bLeft")]
    private bool isLeft = false;
    [UnityEngine.Serialization.FormerlySerializedAs("bRight")]
    private bool isRight = false;
    [UnityEngine.Serialization.FormerlySerializedAs("bMain")]
    private bool isMain = false;
    [UnityEngine.Serialization.FormerlySerializedAs("bUseSharedMaterial")]
    private bool isUsingSharedMaterial = false;
    [UnityEngine.Serialization.FormerlySerializedAs("bLeftTurnYieldOnGreen")]
    private bool isLeftTurnYieldOnGreen = true;
    [UnityEngine.Serialization.FormerlySerializedAs("bLightsEnabled")]
    private bool isLightsEnabled = true;


    #region "Constructor"
    public GSDTrafficLightController(ref GameObject _LightLeft, ref GameObject _LightRight, ref GameObject[] _Lights, ref MeshRenderer _MR_Left, ref MeshRenderer _MR_Right, ref MeshRenderer[] MR_Mains)
    {
        lightLeftObject = _LightLeft;
        lightRightObject = _LightRight;
        lightsObjects = _Lights;

        leftMR = _MR_Left;
        rightMR = _MR_Right;
        mainMRStorage = MR_Mains;
        mainMR = MR_Mains[0];

        Light[] tLights;
        if (lightLeftObject != null)
        {
            tLights = lightLeftObject.transform.GetComponentsInChildren<Light>();
            foreach (Light tLight in tLights)
            {
                if (tLight.transform.name.ToLower().Contains("redlight"))
                {
                    lightLeftR = tLight;
                }
                if (tLight.transform.name.ToLower().Contains("yellowlight"))
                {
                    lightLeftY = tLight;
                }
                if (tLight.transform.name.ToLower().Contains("greenl"))
                {
                    lightLeftG = tLight;
                }
            }
        }
        if (lightRightObject != null)
        {
            tLights = lightRightObject.transform.GetComponentsInChildren<Light>();
            foreach (Light tLight in tLights)
            {
                if (tLight.transform.name.ToLower().Contains("redlight"))
                {
                    lightRightR = tLight;
                }
                if (tLight.transform.name.ToLower().Contains("yellowlight"))
                {
                    lightRightY = tLight;
                }
                if (tLight.transform.name.ToLower().Contains("greenl"))
                {
                    lightRightG = tLight;
                }
            }
        }

        int mCount = lightsObjects.Length;
        lightsR = new Light[mCount];
        lightsY = new Light[mCount];
        lightsG = new Light[mCount];
        for (int index = 0; index < mCount; index++)
        {
            tLights = lightsObjects[index].transform.GetComponentsInChildren<Light>();
            foreach (Light tLight in tLights)
            {
                if (tLight.transform.name.ToLower().Contains("redlight"))
                {
                    lightsR[index] = tLight;
                }
                if (tLight.transform.name.ToLower().Contains("yellowlight"))
                {
                    lightsY[index] = tLight;
                }
                if (tLight.transform.name.ToLower().Contains("greenl"))
                {
                    lightsG[index] = tLight;
                }
            }
        }
    }
    #endregion


    #region "Update"
    public void UpdateLights(iLightStatusEnum _lightStatus, iLightSubStatusEnum _lightSubStatus, bool _isLightsEnabled)
    {
        isLightsEnabled = _isLightsEnabled;
        lightStatus = _lightStatus;
        lightSubStatus = _lightSubStatus;
        isUsingSharedMaterial = false;
        switch (lightStatus)
        {
            case iLightStatusEnum.Regular:
                TriggerRegular();
                break;
            case iLightStatusEnum.LeftTurn:
                TriggerLeftTurn();
                break;
            case iLightStatusEnum.MasterLeft:
                TriggerMasterLeft();
                break;
            case iLightStatusEnum.Red:
                TriggerRed();
                break;
            case iLightStatusEnum.RightTurn:
                TriggerRightTurn();
                break;
        }
    }
    #endregion


    #region "Triggers"
    private void TriggerRegular()
    {
        if (isMain)
        {
            MRChange(ref mainMR, lightSubStatus);
            for (int index = 1; index < mainMRStorage.Length; index++)
            {
                MRChange(ref mainMRStorage[index], lightSubStatus);
            }
            LightChange(0, lightSubStatus);
        }
        if (isLeft)
        {
            if (isLeftTurnYieldOnGreen)
            {
                if (lightSubStatus == iLightSubStatusEnum.Green)
                {
                    MRChangeLeftYield(ref leftMR, iLightYieldSubStatusEnum.Green);
                }
                else if (lightSubStatus == iLightSubStatusEnum.Yellow)
                {
                    MRChangeLeftYield(ref leftMR, iLightYieldSubStatusEnum.Yellow);
                }
            }
            else
            {
                MRChange(ref leftMR, iLightSubStatusEnum.Red);
                LightChange(1, iLightSubStatusEnum.Red);
            }
        }
        if (isRight)
        {
            MRChange(ref rightMR, iLightSubStatusEnum.Red);
            LightChange(2, iLightSubStatusEnum.Red);
        }
    }


    private void TriggerLeftTurn()
    {
        if (isMain)
        {
            MRChange(ref mainMR, iLightSubStatusEnum.Red);
            for (int i = 1; i < mainMRStorage.Length; i++)
            {
                MRChange(ref mainMRStorage[i], iLightSubStatusEnum.Red);
            }
            LightChange(0, iLightSubStatusEnum.Red);
        }
        if (isLeft)
        {
            if (isLeftTurnYieldOnGreen)
            {
                if (lightSubStatus == iLightSubStatusEnum.Green)
                {
                    MRChangeLeftYield(ref leftMR, iLightYieldSubStatusEnum.GreenTurn);
                }
                else if (lightSubStatus == iLightSubStatusEnum.Yellow)
                {
                    MRChangeLeftYield(ref leftMR, iLightYieldSubStatusEnum.YellowTurn);
                }
                LightChange(1, lightSubStatus);
            }
            else
            {
                MRChange(ref leftMR, lightSubStatus);
                LightChange(1, lightSubStatus);
            }
        }
        if (isRight)
        {
            MRChange(ref rightMR, iLightSubStatusEnum.Red);
            LightChange(2, iLightSubStatusEnum.Red);
        }
    }


    private void TriggerMasterLeft()
    {
        if (isMain)
        {
            MRChange(ref mainMR, lightSubStatus);
            for (int index = 1; index < mainMRStorage.Length; index++)
            {
                MRChange(ref mainMRStorage[index], lightSubStatus);
            }
            LightChange(0, lightSubStatus);
        }
        if (isLeft)
        {
            if (lightSubStatus == iLightSubStatusEnum.Green)
            {
                MRChangeLeftYield(ref leftMR, iLightYieldSubStatusEnum.GreenTurn);
            }
            else if (lightSubStatus == iLightSubStatusEnum.Yellow)
            {
                MRChangeLeftYield(ref leftMR, iLightYieldSubStatusEnum.YellowTurn);
            }
            LightChange(1, lightSubStatus);
        }
        if (isRight)
        {
            MRChange(ref rightMR, lightSubStatus);
            LightChange(2, lightSubStatus);
        }
    }


    private void TriggerRightTurn()
    {
        if (isMain)
        {
            MRChange(ref mainMR, iLightSubStatusEnum.Red);
            for (int index = 1; index < mainMRStorage.Length; index++)
            {
                MRChange(ref mainMRStorage[index], iLightSubStatusEnum.Red);
            }
            LightChange(0, iLightSubStatusEnum.Red);
        }
        if (isLeft)
        {
            MRChange(ref leftMR, iLightSubStatusEnum.Red);
            LightChange(1, iLightSubStatusEnum.Red);
        }
        if (isRight)
        {
            MRChange(ref rightMR, lightSubStatus);
            LightChange(2, lightSubStatus);
        }
    }


    private void TriggerRed()
    {
        if (isMain)
        {
            MRChange(ref mainMR, iLightSubStatusEnum.Red);
            for (int index = 1; index < mainMRStorage.Length; index++)
            {
                MRChange(ref mainMRStorage[index], iLightSubStatusEnum.Red);
            }
            LightChange(0, iLightSubStatusEnum.Red);
        }
        if (isLeft)
        {
            MRChange(ref leftMR, iLightSubStatusEnum.Red);
            LightChange(1, iLightSubStatusEnum.Red);
        }
        if (isRight)
        {
            MRChange(ref rightMR, iLightSubStatusEnum.Red);
            LightChange(2, iLightSubStatusEnum.Red);
        }
    }
    #endregion


    private void MRChange(ref MeshRenderer _MR, iLightSubStatusEnum _lightSub)
    {
        if (isUsingSharedMaterial)
        {
            if (_lightSub == iLightSubStatusEnum.Green)
            {
                _MR.sharedMaterial.mainTextureOffset = new Vector2(0.667f, 0f);
            }
            else if (_lightSub == iLightSubStatusEnum.Yellow)
            {
                _MR.sharedMaterial.mainTextureOffset = new Vector2(0.334f, 0f);
            }
            else if (_lightSub == iLightSubStatusEnum.Red)
            {
                _MR.sharedMaterial.mainTextureOffset = new Vector2(0f, 0f);
            }
        }
        else
        {
            if (_lightSub == iLightSubStatusEnum.Green)
            {
                _MR.material.mainTextureOffset = new Vector2(0.667f, 0f);
            }
            else if (_lightSub == iLightSubStatusEnum.Yellow)
            {
                _MR.material.mainTextureOffset = new Vector2(0.334f, 0f);
            }
            else if (_lightSub == iLightSubStatusEnum.Red)
            {
                _MR.material.mainTextureOffset = new Vector2(0f, 0f);
            }
        }
    }


    private void MRChangeLeftYield(ref MeshRenderer _MR, iLightYieldSubStatusEnum _lightYieldSub)
    {
        if (isUsingSharedMaterial)
        {
            if (_lightYieldSub == iLightYieldSubStatusEnum.Green)
            {
                _MR.sharedMaterial.mainTextureOffset = new Vector2(0.667f, 0f);
            }
            else if (_lightYieldSub == iLightYieldSubStatusEnum.Yellow)
            {
                _MR.sharedMaterial.mainTextureOffset = new Vector2(0.334f, 0f);
            }
            else if (_lightYieldSub == iLightYieldSubStatusEnum.Red)
            {
                _MR.sharedMaterial.mainTextureOffset = new Vector2(0f, 0f);
            }
            else if (_lightYieldSub == iLightYieldSubStatusEnum.YellowTurn)
            {
                _MR.sharedMaterial.mainTextureOffset = new Vector2(0.6f, 0f);
            }
            else if (_lightYieldSub == iLightYieldSubStatusEnum.GreenTurn)
            {
                _MR.sharedMaterial.mainTextureOffset = new Vector2(0.8f, 0f);
            }
        }
        else
        {
            if (_lightYieldSub == iLightYieldSubStatusEnum.Green)
            {
                _MR.material.mainTextureOffset = new Vector2(0.4f, 0f);
            }
            else if (_lightYieldSub == iLightYieldSubStatusEnum.Yellow)
            {
                _MR.material.mainTextureOffset = new Vector2(0.2f, 0f);
            }
            else if (_lightYieldSub == iLightYieldSubStatusEnum.Red)
            {
                _MR.material.mainTextureOffset = new Vector2(0f, 0f);
            }
            else if (_lightYieldSub == iLightYieldSubStatusEnum.YellowTurn)
            {
                _MR.material.mainTextureOffset = new Vector2(0.6f, 0f);
            }
            else if (_lightYieldSub == iLightYieldSubStatusEnum.GreenTurn)
            {
                _MR.material.mainTextureOffset = new Vector2(0.8f, 0f);
            }
        }
    }


    private void LightChange(int _index, iLightSubStatusEnum _lightSub)
    {
        if (!isLightsEnabled)
        {
            int meshCount = mainMRStorage.Length;
            for (int index = 0; index < meshCount; index++)
            {
                lightsR[index].enabled = false;
                lightsY[index].enabled = false;
                lightsG[index].enabled = false;
            }
            if (lightLeftR != null)
            {
                lightLeftR.enabled = false;
            }
            if (lightLeftY != null)
            {
                lightLeftY.enabled = false;
            }
            if (lightLeftG != null)
            {
                lightLeftG.enabled = false;
            }
            if (lightRightR != null)
            {
                lightRightR.enabled = false;
            }
            if (lightRightY != null)
            {
                lightRightY.enabled = false;
            }
            if (lightRightG != null)
            {
                lightRightG.enabled = false;
            }
            return;
        }

        if (_index == 0)
        {
            //Main:
            int mCount = mainMRStorage.Length;
            for (int index = 0; index < mCount; index++)
            {
                LightChangeHelper(ref lightsR[index], ref lightsY[index], ref lightsG[index], _lightSub);
            }
        }
        else if (_index == 1)
        {
            //Left:
            LightChangeHelper(ref lightLeftR, ref lightLeftY, ref lightLeftG, _lightSub);
        }
        else if (_index == 2)
        {
            //Right:
            LightChangeHelper(ref lightRightR, ref lightRightY, ref lightRightG, _lightSub);
        }
    }


    private void LightChangeHelper(ref Light _red, ref Light _yellow, ref Light _green, iLightSubStatusEnum _lightSub)
    {
        if (_lightSub == iLightSubStatusEnum.Green)
        {
            _red.enabled = false;
            _yellow.enabled = false;
            _green.enabled = true;
        }
        else if (_lightSub == iLightSubStatusEnum.Yellow)
        {
            _red.enabled = false;
            _yellow.enabled = true;
            _green.enabled = false;
        }
        else if (_lightSub == iLightSubStatusEnum.Red)
        {
            _red.enabled = true;
            _yellow.enabled = false;
            _green.enabled = false;
        }
    }


    #region "Setup"
    public void Setup(bool _isLeftYield)
    {
        SetupObject(leftMR);
        SetupObject(rightMR);
        SetupMainObjects();
        isLeft = (leftMR != null);
        isRight = (rightMR != null);
        isMain = (mainMR != null);
        isLeftTurnYieldOnGreen = _isLeftYield;
    }


    private void SetupMainObjects()
    {
        if (mainMR == null)
        {
            return;
        }
        int meshCount = mainMRStorage.Length;
        if (meshCount == 0)
        {
            return;
        }
        SetupObject(mainMR);
        if (meshCount > 1)
        {
            for (int index = 1; index < meshCount; index++)
            {
                if (isUsingSharedMaterial)
                {
                    mainMRStorage[index].sharedMaterial = mainMR.sharedMaterial;
                }
                else
                {
                    Material[] materials = new Material[1];
                    materials[0] = mainMR.materials[0];
                    mainMRStorage[index].materials = materials;

                }

            }
        }
    }


    private void SetupObject(MeshRenderer _MR)
    {
        if (_MR != null)
        {
            _MR.material = _MR.material;
        }
    }
    #endregion
}


public class GSDTrafficLightSequence
{
    [UnityEngine.Serialization.FormerlySerializedAs("bLightMasterPath1")]
    public bool isLightMasterPath1 = true;
    [UnityEngine.Serialization.FormerlySerializedAs("iLightController")]
    public GSDTrafficLightController.iLightControllerEnum lightController = GSDTrafficLightController.iLightControllerEnum.Regular;
    [UnityEngine.Serialization.FormerlySerializedAs("iLightSubcontroller")]
    public GSDTrafficLightController.iLightSubStatusEnum lightSubcontroller = GSDTrafficLightController.iLightSubStatusEnum.Green;
    [UnityEngine.Serialization.FormerlySerializedAs("tTime")]
    public float time = 10f;


    public GSDTrafficLightSequence(bool _isPath1, GSDTrafficLightController.iLightControllerEnum _lightController, GSDTrafficLightController.iLightSubStatusEnum _lightSubcontroller, float _time)
    {
        isLightMasterPath1 = _isPath1;
        lightController = _lightController;
        lightSubcontroller = _lightSubcontroller;
        time = _time;
    }


    public string ToStringRA()
    {
        return "Path1: " + isLightMasterPath1 + " iLightController: " + lightController.ToString() + " iLightSubcontroller: " + lightSubcontroller.ToString() + " tTime: " + time.ToString("0F");
    }
}