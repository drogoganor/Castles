using ImGuiNET;
using System.Numerics;
using System;
using Castles.Interfaces;
using Castles.Providers;
using Castles.Render;
using static System.Formats.Asn1.AsnWriter;
using Castles.Enums;

namespace Castles.UI
{
    public class EditorUI : Menu
    {
        public event Action OnExit;

        private readonly Scene scene;

        public EditorUI(
            ModManifestProvider modManifestProvider,
            IApplicationWindow window,
            Scene scene) : base(modManifestProvider, window)
        {
            this.scene = scene;
        }

        public override void Show()
        {
            base.Show();
            scene.Show();
        }

        public override void Hide()
        {
            base.Hide();
            scene.Hide();
        }

        private void HandleExit()
        {
            Hide();
            OnExit?.Invoke();
        }

        protected override void Draw(float deltaSeconds)
        {
            PreDraw(deltaSeconds);

            var windowSize = new Vector2(Window.Width, Window.Height);
            var menuSize = new Vector2(400, 600);
            ImGui.SetNextWindowSize(menuSize);

            ImGui.SetNextWindowPos(new Vector2(0, 0));
            ImGui.PushFont(Fonts[FontSize.Small].Value);

            if (ImGui.Begin("Tools"))
            {
                ImGui.Text("Tools here");
            }

            base.Draw(deltaSeconds);
        }
    }
}
