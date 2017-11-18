using System;

namespace IdiotGraphics.BasicTypes
{
  /// <summary>
  ///   Much like the CSS property 'border'. Contains the borders size, color and radius.
  /// </summary>
  public class BorderStyle
  {
    #region Fields / Properties

    public BorderSize Size;
    public Color Color;
    public float Radius;

    #endregion

    public BorderStyle(BorderSize size, Color color, float radius)
    {
      Size = size;
      Color = color;
      Radius = radius;
    }

    public BorderStyle(BorderSize size, Color color) : this(size, color, 0)
    {
    }
  }

  /// <summary>
  ///   The border's sizes (all 4 of them).
  /// </summary>
  public class BorderSize
  {
    #region Fields / Properties

    /// <summary>
    ///   Evaluates to true only if all values are the same (even 0).
    /// </summary>
    public bool IsUniform => Left == Top && Left == Right && Left == Bottom;

    public float Top;
    public float Right;
    public float Bottom;
    public float Left;

    #endregion

    public BorderSize(float size) : this(size, size, size, size)
    {
    }

    public BorderSize(float top, float right, float bottom, float left)
    {
      Top = top;
      Right = right;
      Bottom = bottom;
      Left = left;
    }

    public BorderSize(params float[] values)
    {
      switch (values.Length)
      {
        case 1:
          Top = Right = Bottom = Left = values[0];
          break;
        case 2:
          Top = Bottom = values[0];
          Left = Right = values[1];
          break;
        case 4:
          Top = values[0];
          Right = values[1];
          Bottom = values[2];
          Left = values[3];
          break;
        default:
          throw new Exception("BorderSize from int array must be 1, 2, or 4 values.");
      }
    }

    public static implicit operator BorderSize(float value) => new BorderSize(value);

    public static implicit operator BorderSize(float[] values) => new BorderSize(values);

    public void CopyFrom(BorderSize borderSize)
    {
      Top = borderSize.Top;
      Right = borderSize.Right;
      Bottom = borderSize.Bottom;
      Left = borderSize.Left;
    }

    #region Equality

    public bool Equals(BorderSize other) =>
      !ReferenceEquals(other, null) &&
      Top.Equals(other.Top) && Right.Equals(other.Right) &&
      Bottom.Equals(other.Bottom) && Left.Equals(other.Left);

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      return obj is BorderSize size && Equals(size);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        var hashCode = Top.GetHashCode();
        hashCode = (hashCode * 397) ^ Right.GetHashCode();
        hashCode = (hashCode * 397) ^ Bottom.GetHashCode();
        hashCode = (hashCode * 397) ^ Left.GetHashCode();
        return hashCode;
      }
    }

    #endregion

    #region Operators

    public static bool operator ==(BorderSize left, BorderSize right) =>
      ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.Equals(right);

    public static bool operator !=(BorderSize left, BorderSize right) =>
      ReferenceEquals(left, null) ? !ReferenceEquals(right, null) : !left.Equals(right);

    public static BorderSize operator +(BorderSize l, BorderSize r) =>
      new BorderSize(l.Left + r.Left, l.Top + r.Top, l.Right + r.Right, l.Bottom + r.Bottom);

    public static BorderSize operator -(BorderSize l, BorderSize r) =>
      l + new BorderSize(-r.Left, -r.Top, -r.Right, -r.Bottom);

    #endregion
  }
}