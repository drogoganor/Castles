#version 450

layout(location = 0) in vec3 fsin_texCoords;
layout(location = 0) out vec4 fsout_color;

layout(set = 1, binding = 1) uniform texture2DArray SurfaceTexture;
layout(set = 1, binding = 2) uniform sampler SurfaceSampler;

void main()
{
    fsout_color =  texture(sampler2DArray(SurfaceTexture, SurfaceSampler), fsin_texCoords);
}
