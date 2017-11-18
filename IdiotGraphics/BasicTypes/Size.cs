namespace IdiotGraphics.BasicTypes
{
  /// <summary>
  ///   A size in Width, Height
  /// </summary>
  public class Size
  {
    #region Fields / Properties

    public float Width;
    public float Height;

    #endregion

    public Size()
    {
    }

    public Size(float width, float height)
    {
      Width = width;
      Height = height;
    }

    public Size(float both) : this(both, both)
    {
    }

    public override string ToString() => $"{{Width={Width}, Height={Height}";

    #region System.Drawing

    public static implicit operator System.Drawing.Size(Size size) =>
      new System.Drawing.Size((int) size.Width, (int) size.Height);

    public static implicit operator Size(System.Drawing.Size size) => new Size(size.Width, size.Height);

    #endregion

    #region Equality

    public bool Equals(Size other) =>
      !ReferenceEquals(other, null) &&
      Width.Equals(other.Width) && Height.Equals(other.Height);

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      return obj is Size size && Equals(size);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        return (Width.GetHashCode() * 397) ^ Height.GetHashCode();
      }
    }

    #endregion

    #region Operators

    public static bool operator ==(Size left, Size right) => left.Equals(right);

    public static bool operator !=(Size left, Size right) => !left.Equals(right);

    #endregion
  }
}