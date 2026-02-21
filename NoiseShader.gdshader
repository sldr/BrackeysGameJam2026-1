shader_type canvas_item;

uniform sampler2D noise_texture;

void fragment() {
	vec4 basetex = texture(TEXTURE, UV);
	float noise = texture(noise_texture, UV).r;
    
	float mask = step(0.5, noise);
	COLOR = basetex;
    COLOR.a = basetex.a * mask; 
}
