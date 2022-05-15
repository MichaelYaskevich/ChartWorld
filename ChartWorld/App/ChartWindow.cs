using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Threading;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace ChartWorld.App
{
    public sealed partial class ChartWindow : Form
    {
        public static Queue<Action<Graphics>> ToPaint { get; } = new();
        private static Workspace.Workspace Workspace { get; set; }

        public ChartWindow(Workspace.Workspace workspace)
        {
            Workspace = workspace;
            InitializeComponent();
            DoubleBuffered = true;
            SettingsLoader.LoadDefaultSettings(this);
            ChartSettings.InitializeStartButton(this, workspace);
            KeyDown += OnKeyDown;
            SetStyle(ControlStyles.ResizeRedraw, true);
            Click += (sender, args) =>
            {
                // Focus();
                workspace.ChosenEntity = null;
            };

            var drawingTimer = new Timer();
            drawingTimer.Interval = 30;
            drawingTimer.Tick += (s,a) => {
                if (workspace.WasModified)
                {
                    Update();
                    workspace.WasModified = false;
                }
            };
            drawingTimer.Start();
        }

        private void OnKeyDown(object? sender, KeyEventArgs e)
        {
            if (Workspace.ChosenEntity != null)
                ToolsForActions.MakeEntityAction(e.KeyCode, Workspace.ChosenEntity);
            else
                ToolsForActions.MakeWorkspaceAction(e.KeyCode, Workspace);
            
            Update();
        }

        public void Update()
        {
            Invalidate();
            foreach (var entity in Workspace.GetWorkspaceEntities())
                Painter.Paint(entity, this);
        }
        

        protected override void OnPaint(PaintEventArgs e)
        {
            var graphics = e.Graphics;
            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            while (ToPaint.Count > 0)
            {
                var action = ToPaint.Dequeue();
                action(graphics);
            }
        }
    }
}