using System.Runtime.InteropServices;

namespace Shimakaze.UI.Media.Cursor;

/// <summary>
/// 动画头
/// </summary>
/// <param name="Sizeof">结构体大小</param>
/// <param name="Frames">图像帧数</param>
/// <param name="Steps">播放帧数，当'seq '存在时可能大于dwNumFrames</param>
/// <param name="Width">图像宽度</param>
/// <param name="Height">图像高度</param>
/// <param name="BitCount">色彩位数</param>
/// <param name="Planes">设备平面数</param>
/// <param name="JifRate">显示频率（Time Delay，单位为1/60秒）</param>
/// <param name="Flags">标志</param>
[StructLayout(LayoutKind.Explicit, Pack = 1)]
public readonly record struct AniHeader(
    [field: FieldOffset(sizeof(uint) * 0)] uint Sizeof,
    [field: FieldOffset(sizeof(uint) * 1)] uint Frames,
    [field: FieldOffset(sizeof(uint) * 2)] uint Steps,
    [field: FieldOffset(sizeof(uint) * 3)] uint Width,
    [field: FieldOffset(sizeof(uint) * 4)] uint Height,
    [field: FieldOffset(sizeof(uint) * 5)] uint BitCount,
    [field: FieldOffset(sizeof(uint) * 6)] uint Planes,
    [field: FieldOffset(sizeof(uint) * 7)] uint JifRate,
    [field: FieldOffset(sizeof(uint) * 8)] uint Flags
);