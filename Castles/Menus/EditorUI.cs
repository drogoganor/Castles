using ImGuiNET;
using System.Numerics;
using System;
using Castles.Interfaces;
using Castles.Providers;
using Castles.Enums;

namespace Castles.UI
{
    public class EditorUI : Menu
    {
        public event Action OnExit;

        public EditorUI(
            IApplicationWindow window,
            ImGuiProvider imGuiProvider,
            GraphicsDeviceProvider graphicsDeviceProvider) : base(window, imGuiProvider, graphicsDeviceProvider)
        {
        }

        private void HandleExit()
        {
            OnExit?.Invoke();
        }

        public override void Draw(float deltaSeconds)
        {
            UpdateInput(deltaSeconds);

            var windowSize = new Vector2(window.Width, window.Height);
            var menuSize = new Vector2(400, 600);
            ImGui.SetNextWindowSize(menuSize);

            ImGui.SetNextWindowPos(new Vector2(0, 0));
            ImGui.PushFont(imGuiProvider.Fonts[FontSize.Small].Value);

            if (ImGui.Begin("Tools"))
            {
                ImGui.Text("Tools here");
            }

            base.Draw(deltaSeconds);
        }
    }
}
