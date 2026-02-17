shader_type canvas_item;



void fragment() {
	float circle = distance(vec2(0.5), UV);
	COLOR.rgb = vec3(1);
	COLOR.a = step(circle, 0.5);
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
