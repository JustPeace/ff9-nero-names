using System;
using System.Threading;
using Memoria.Prime;
using UnityEngine;

namespace NeroNames
{
    // ScriptsLoader reads all custom attributes on its worker thread. Register
    // for the existing main-thread update event; never call Unity there.
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class StartOverlayAttribute : Attribute
    {
        private static int registered;
        public StartOverlayAttribute()
        {
            if (Interlocked.Exchange(ref registered, 1) == 0)
                GameLoopManager.Update += StartOnMainThread;
        }

        private static void StartOnMainThread()
        {
            try
            {
                GameObject host = new GameObject("NeroNames.LocalOverlay");
                UnityEngine.Object.DontDestroyOnLoad(host);
                host.AddComponent<NeroNamesOverlay>();
                GameLoopManager.Update -= StartOnMainThread;
                Log.Message("[NeroNames] Loaded v1.0. Map 1858; SID 4=Zenero, 7=Benero, 8=Genero.");
            }
            catch (Exception ex)
            {
                GameLoopManager.Update -= StartOnMainThread;
                Log.Error(ex, "[NeroNames] Failed to start the name overlay.");
            }
        }
    }

    [StartOverlay]
    public sealed class Bootstrap { }

    public sealed class NeroNamesOverlay : MonoBehaviour
    {
        private FieldMapActorController[] actors = new FieldMapActorController[0];
        private float nextScan;
        private GUIStyle style;
        private Font font;
        private bool reported;
        private static readonly Color[] Colours = {
            new Color(1f, .86f, .30f), new Color(.40f, .95f, 1f), new Color(1f, .62f, .85f)
        };

        public static int NameIndex(int map, int sid)
        {
            if (map != 1858) return -1;
            if (sid == 4) return 0;
            if (sid == 7) return 1;
            if (sid == 8) return 2;
            return -1;
        }

        public static string NameFor(int index)
        {
            switch (index)
            {
                case 0: return "泽内罗";
                case 1: return "贝内罗";
                case 2: return "格内罗";
                default: return "";
            }
        }

        private void OnGUI()
        {
            try
            {
                if (FF9StateSystem.Common.FF9.fldMapNo != 1858)
                {
                    actors = new FieldMapActorController[0];
                    nextScan = 0;
                    return;
                }
                if (PersistenSingleton<EventEngine>.Instance.gMode != 1) return;
                if (Time.realtimeSinceStartup >= nextScan)
                {
                    actors = UnityEngine.Object.FindObjectsOfType<FieldMapActorController>();
                    nextScan = Time.realtimeSinceStartup + .5f;
                }
                if (style == null)
                {
                    font = Font.CreateDynamicFontFromOSFont(new string[] { "Microsoft YaHei", "SimHei", "Arial" }, 20);
                    style = new GUIStyle(GUI.skin.box);
                    style.font = font;
                    style.alignment = TextAnchor.MiddleCenter;
                    style.fontStyle = FontStyle.Bold;
                    style.wordWrap = false;
                }
                float scale = Mathf.Max(.65f, Screen.height / 720f);
                style.fontSize = Mathf.RoundToInt(18f * scale);
                foreach (FieldMapActorController actor in actors)
                {
                    if (actor == null || actor.originalActor == null || actor.fieldMap == null || !actor.gameObject.activeInHierarchy)
                        continue;
                    int index = NameIndex(1858, actor.originalActor.sid);
                    if (index < 0 || actor.originalActor.model != 5500) continue;
                    if (!actor.IsVisibled() || !actor.IsActive()) continue;
                    Camera camera = actor.fieldMap.GetMainCamera();
                    BGCAM_DEF background = actor.fieldMap.GetCurrentBgCamera();
                    if (camera == null || background == null) continue;
                    // Match the engine's PSX projection (PSX itself is internal).
                    Vector3 vertex = actor.transform.position;
                    vertex.y = -vertex.y;
                    Vector3 projected = background.GetMatrixRT().MultiplyPoint(vertex);
                    projected.y = -projected.y;
                    float depth = Mathf.Abs(projected.z);
                    if (depth < .001f) continue;
                    Vector2 offset = actor.fieldMap.GetProjectionOffset();
                    float perspective = background.GetViewDistance() / depth;
                    projected.x = projected.x * perspective + offset.x;
                    projected.y = projected.y * perspective + offset.y;
                    projected.z = 0;
                    Vector3 point = camera.WorldToScreenPoint(projected);
                    float x = point.x - 49f * scale;
                    float y = Screen.height - point.y - 88f * scale;
                    if (Single.IsNaN(x) || Single.IsNaN(y) || Single.IsInfinity(x) || Single.IsInfinity(y)) continue;
                    style.normal.textColor = Colours[index];
                    GUI.Box(new Rect(x, y, 98f * scale, 30f * scale), NameFor(index), style);
                }
            }
            catch (Exception ex)
            {
                if (!reported) { reported = true; Log.Error(ex, "[NeroNames] Could not draw labels."); }
            }
        }

        private void OnDestroy()
        {
            if (font != null) UnityEngine.Object.Destroy(font);
        }
    }
}
