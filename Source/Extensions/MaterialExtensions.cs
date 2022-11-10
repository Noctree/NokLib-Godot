using Godot;

namespace NokLib_Godot;
public static class MaterialExtensions
{
    public static MaterialType GetMaterialType(this Material material) {
        return material switch {
            ShaderMaterial _ => MaterialType.Shader,
            CanvasItemMaterial _ => MaterialType.CanvasItem,
            ParticleProcessMaterial _ => MaterialType.ParticleProcess,
            StandardMaterial3D _ => MaterialType.Standard,
            _ => MaterialType.Unknown
        };
    }
}