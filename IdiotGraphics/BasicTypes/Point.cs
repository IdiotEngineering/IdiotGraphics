namespace IdiotGraphics.BasicTypes
{
  /// <summary>
  ///   A point in space in Left, Top, or X, Y coordinates.
  /// </summary>
  public class Point
  {
    #region Fields / Properties

    public float X { get; set; }
    public float Y { get; set; }

    public float Left
    {
      get => X;
      set => X = value;
    }

    public float Top
    {
      get => Y;
      set => Y = value;
    }

    #endregion

    public Point()
    {
    }

    public Point(float x, float y)
    {
      X = x;
      Y = y;
    }

    public override string ToString() => $"{{X={X}, Y={Y}}}";

    #region Equality

    public bool Equals(Point other) =>
      !ReferenceEquals(other, null) && X.Equals(other.X) && Y.Equals(other.Y);

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      return obj is Point point && Equals(point);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        return (X.GetHashCode() * 397) ^ Y.GetHashCode();
      }
    }

    #endregion

    #region Operators

    public static bool operator ==(Point left, Point right) =>
      ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.Equals(right);

    public static bool operator !=(Point left, Point right) =>
      ReferenceEquals(left, null) ? !ReferenceEquals(right, null) : !left.Equals(right);

    public static Point operator +(Point left, Point right) =>
      new Point(left.X + right.X, left.Y + right.Y);

    public static Point operator -(Point left, Point right) =>
      new Point(left.X - right.X, left.Y - right.Y);

    #endregion

    #region System.Drawing

    public static implicit operator System.Drawing.Point(Point point) =>
      new System.Drawing.Point((int) point.X, (int) point.Y);

    public static implicit operator Point(System.Drawing.Point point) => new Point(point.X, point.Y);

    #endregion
  }
}