shader_type canvas_item;

uniform sampler2D texture1;
uniform sampler2D texture2;
uniform float mix_amount : hint_range(0, 1) = 0.5;

void fragment() {
	
	vec4 col1 = texture(texture1, UV);
    vec4 col2 = texture(texture2, UV);
    // Mix based on mix_amount
    COLOR = mix(col1, col2, mix_amount);
}
