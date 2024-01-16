shader_type canvas_item;

uniform int amount = 5;

void fragment() {
	
	vec4 col = COLOR;
	
	col = vec4(col.r * float(amount), col.g * float(amount), col.b * float(amount), 1.0);
	col = floor(col);
	col = vec4(col.r / float(amount), col.g / float(amount), col.b / float(amount), 1.0);
	
	COLOR = col;
	
}

//void light() {
	// Called for every pixel for every light affecting the material.
	// Uncomment to replace the default light processing function with this one.
//}
