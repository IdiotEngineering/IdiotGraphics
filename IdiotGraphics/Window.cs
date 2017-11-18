using System;
using IdiotGraphics.BasicTypes;
using OpenTK;
using OpenTK.Graphics;

namespace IdiotGraphics
{
  public enum WindowBorder
  {
    Hidden,
    Fixed,
    Resizable
  }

  /// <summary>
  ///   An OS window managed and drawn by IdiotGui
  /// </summary>
  public class Window : IDisposable
  {
    #region Fields / Properties

    /// <summary>
    ///   Fired AFTER the window is resized (not during resize)
    /// </summary>
    public event EventHandler<EventArgs> Resized;

    /// <summary>
    ///   Fired when the window is closing.
    /// </summary>
    public event EventHandler<EventArgs> Closing;

    public bool SupressNextInputEvents;

    public WindowBorder NativeWindowBorder
    {
      get => _nativeWindowNativeWindowBorder;
      set
      {
        _nativeWindowNativeWindowBorder = value;
        if (NativeWindow == null) return;
        NativeWindow.WindowBorder = ToOpenTkBorder(value);
      }
    }

    /// <summary>
    ///   The location (in screen-space, pixel coordinates) of the window.
    /// </summary>
    public Point WindowLocation
    {
      get => NativeWindow.Location;
      set => NativeWindow.Location = value;
    }

    public Size WindowClientSize
    {
      get => NativeWindow.ClientSize;
      set
      {
        NativeWindow.ClientSize = value;
        UpdateScreenSize();
      }
    }

    public bool Visiable
    {
      get => NativeWindow.Visible;
      set => NativeWindow.Visible = value;
    }

    internal readonly NativeWindow NativeWindow;
    private GraphicsContext _glGraphicsContext;
    private WindowBorder _nativeWindowNativeWindowBorder = WindowBorder.Resizable;
    private Point _lastDragPoint;

    #endregion

    public Window(string title, int width, int height)
    {
      // Native window and Render Context initialize
      NativeWindow = new NativeWindow(width, height, title, GameWindowFlags.Default, GraphicsMode.Default,
        DisplayDevice.Default)
      {
        Visible = false,
        Title = title
      };
      RegisterEventHandlers();
      // GL Context
      _glGraphicsContext =
        new GraphicsContext(GraphicsMode.Default, NativeWindow.WindowInfo, 4, 2, GraphicsContextFlags.Debug);
      _glGraphicsContext.LoadAll();
      _glGraphicsContext.MakeCurrent(NativeWindow.WindowInfo);
      // Register NativeWindow events
      //NativeWindow.KeyDown += (sender, args) => FocusedElement?.OnKeyDown(args);
      //NativeWindow.KeyUp += (sender, args) => FocusedElement?.OnKeyUp(args);
      //window.NativeWindow.Resized += (sender, args) => _glTexture.Resized(_window.UnscaledSize.Width, _window.UnscaledSize.Height);
      //NativeWindow.MouseDown += (sender, args) =>
      //{
      //  if (SupressNextInputEvents) return;
      //  var clickedElement = GetTopmostElementAtPoint(args.Position);
      //  if (clickedElement == FocusedElement) return;
      //  // De-Focus last focused element
      //  FocusedElement?.OnLostFocus();
      //  // Focus the new one
      //  FocusedElement = clickedElement;
      //  clickedElement.OnFocus();
      //  // Mouse down
      //  var mouseState = Mouse.GetCursorState();
      //  _lastDragPoint = new Point(mouseState.X, mouseState.Y);
      //  clickedElement.OnMouseDown(args);
      //};
      //NativeWindow.MouseUp += (sender, args) =>
      //{
      //  if (SupressNextInputEvents) return;
      //  var clickedElement = GetTopmostElementAtPoint(args.Position);
      //  // If the mouse is still within the control's bounds fire click event
      //  if (clickedElement == FocusedElement) FocusedElement?.OnClicked(args);
      //  clickedElement?.OnMouseUp(args);
      //  _lastDragPoint = null;
      //};
      //NativeWindow.MouseMove += (sender, args) =>
      //{
      //  if (SupressNextInputEvents) return;
      //  // NativeWindow.Title = args.Position.ToString();
      //  var mouseOverElement = GetTopmostElementAtPoint(args.Position);
      //  // Dragging
      //  if (_lastDragPoint != null)
      //  {
      //    var mouseState = Mouse.GetCursorState();
      //    var newPoint = new Point(mouseState.X, mouseState.Y);
      //    var mouseDragArgs = new MouseDraggedArgs { Delta = newPoint - _lastDragPoint };
      //    mouseOverElement?.OnMouseDrag(mouseDragArgs);
      //    if (mouseDragArgs.RequestCaptureMouse)
      //    {
      //      Console.WriteLine("Moving to: " + _lastDragPoint.X + " : " + _lastDragPoint.Y);
      //      Mouse.SetPosition(_lastDragPoint.X, _lastDragPoint.Y);
      //    }
      //    else
      //      _lastDragPoint = newPoint;
      //  }
      //  // MouseOver tracking
      //  if (mouseOverElement == _lastMouseOver) return;
      //  _lastMouseOver?.OnMouseLeave();
      //  _lastMouseOver = mouseOverElement;
      //  mouseOverElement?.OnMouseEnter();
      //};
      //NativeWindow.MouseWheel += (sender, args) =>
      //{
      //  if (SupressNextInputEvents) return;
      //  FocusedElement?.OnMouseWheel(args);
      //};
      //NativeWindow.FocusedChanged += (sender, args) =>
      //{
      //  if (SupressNextInputEvents) return;
      //  if (NativeWindow.Focused)
      //  {
      //    // Refocused
      //    FocusedElement?.OnFocus();
      //  }
      //  else
      //  {
      //    // Unfocused
      //    FocusedElement?.OnLostFocus();
      //    _lastMouseOver?.OnMouseLeave();
      //  }
      //};
      //// A few default for the window Element
      //Background = Theme.Colors.DefaultFill;
      //Margin = 0;
      //Border = new BorderStyle(0, Theme.Colors.DefaultBorder);
      //Padding = 0;
    }

    public void Dispose()
    {
      _glGraphicsContext?.Dispose();
      _glGraphicsContext = null;
    }

    public void Update()
    {
      NativeWindow.ProcessEvents();
      UpdateScreenSize();
      SupressNextInputEvents = false;
    }

    public void Render()
    {
      _glGraphicsContext.MakeCurrent(NativeWindow.WindowInfo);
      _glGraphicsContext.SwapBuffers();
    }

    public void Close()
    {
      NativeWindow.Close();
    }

    private void RegisterEventHandlers()
    {
      // Resized
      NativeWindow.Resize += (sender, args) =>
      {
        // Note that GUI stays in scaled coordinates
        UpdateScreenSize();
        Resized?.Invoke(this, args);
      };
      // Closing
      NativeWindow.Closing += (sender, args) =>
      {
        _glGraphicsContext.Dispose();
        _glGraphicsContext = null;
        Closing?.Invoke(this, args);
      };
    }

    private void UpdateScreenSize()
    {
    }

    private static OpenTK.WindowBorder ToOpenTkBorder(WindowBorder value)
    {
      switch (value)
      {
        case WindowBorder.Resizable:
          return OpenTK.WindowBorder.Resizable;
        case WindowBorder.Fixed:
          return OpenTK.WindowBorder.Fixed;
        case WindowBorder.Hidden:
          return OpenTK.WindowBorder.Hidden;
        default:
          throw new ArgumentOutOfRangeException(nameof(value), value, null);
      }
    }
  }
}