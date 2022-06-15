using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using ChartWorld.App;
using ChartWorld.Domain.Statistic;
using ChartWorld.Domain.Workspace;
using Timer = System.Windows.Forms.Timer;

namespace ChartWorld.UI
{
    public sealed partial class ChartWindow : Form
    {
        public static Queue<Action<Graphics>> ToPaint { get; } = new();
        private static Workspace Workspace { get; set; }
        public static ICommandsExecutor Executor { get; set; }

        public ChartWindow(Workspace workspace, ICommandsExecutor executor)
        {
            Workspace = workspace;
            Executor = executor;
            InitializeComponent();
            DoubleBuffered = true;
            SettingsLoader.LoadDefaultSettings(this);
            ChartSettings.InitializeStartButtons(this, workspace);
            KeyDown += OnKeyDown;
            SetStyle(ControlStyles.ResizeRedraw, true);
            Click += (_, _) => { workspace.SelectedEntity = null; };

            var drawingTimer = new Timer();
            drawingTimer.Interval = 30;
            drawingTimer.Tick += (_, _) =>
            {
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
            if (Workspace.SelectedEntity != null)
                ToolsForActions.MakeEntityAction(
                    e.KeyCode, Workspace.SelectedEntity, Workspace.SelectionType);

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