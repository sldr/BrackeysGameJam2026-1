shader_type canvas_item;
const vec2 center = vec2(0.5);
uniform float radius : hint_range(0.0, 1.0) = 0.5;

void fragment() {
	float circle_main = step(distance(UV, center), radius);
	COLOR = vec4(circle_main);
}
