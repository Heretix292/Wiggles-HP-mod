using KSL.API;
using CarX;
using UnityEngine;

namespace Wiggles_hpmodv2
{
    public class HPMod : BaseMod
    {
        private bool showMenu = false;
        private Rect windowRect = new Rect(30, 30, 320, 200);

        private float maxTorque = 400f;
        private float torqueRPM = 4000f;
        private float revLimiter = 8000f;

        private const float MIN_TORQUE = 50f;
        private const float MAX_TORQUE = 3000f;
        private const float MIN_RPM = 1000f;
        private const float MAX_RPM = 10000f;
        private const float MIN_REV = 3000f;
        private const float MAX_REV = 15000f;

        private string status = "";

        public new KSLMeta Meta { get; } = new KSLMeta("com.wiggles.hpmod", "Wiggles HP Mod", "1.0.0");

        public override void OnUIDraw()
        {
            if (Input.GetKeyDown(KeyCode.F8))
                showMenu = !showMenu;

            if (showMenu)
                windowRect = GUI.Window(9877, windowRect, DrawWindow, "Engine Tuner [F8]");
        }

        private void DrawWindow(int id)
        {
            GUILayout.Space(5);

            GUILayout.Label("Max Torque: " + (int)maxTorque + " Nm");
            maxTorque = GUILayout.HorizontalSlider(maxTorque, MIN_TORQUE, MAX_TORQUE);

            GUILayout.Space(6);

            GUILayout.Label("Torque RPM: " + (int)torqueRPM);
            torqueRPM = GUILayout.HorizontalSlider(torqueRPM, MIN_RPM, MAX_RPM);

            GUILayout.Space(6);

            GUILayout.Label("Rev Limiter: " + (int)revLimiter + " RPM");
            revLimiter = GUILayout.HorizontalSlider(revLimiter, MIN_REV, MAX_REV);

            GUILayout.Space(10);

            if (GUILayout.Button("Apply"))
                ApplyStats();

            if (status != "")
            {
                GUILayout.Space(4);
                GUILayout.Label(status);
            }

            GUI.DragWindow();
        }

        private void ApplyStats()
        {
            var car = GameObject.FindObjectOfType<CARXCar>();
            if (car == null)
            {
                status = "No car found! Get in a car first.";
                return;
            }

            try
            {
                car.SetEngineMaxTorque(maxTorque, torqueRPM);
                car.engineRevLimiter = revLimiter;
                car.ApplyCarDesc();
                status = "Applied! Torque=" + (int)maxTorque + " Rev=" + (int)revLimiter;
            }
            catch (System.Exception ex)
            {
                status = "Error: " + ex.Message;
            }
        }
    }
}