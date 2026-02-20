shader_type canvas_item;

uniform sampler2D noise;

void fragment() {
	float texture_alpha = texture(noise, UV).r;
	COLOR.a = texture_alpha;
	
}

