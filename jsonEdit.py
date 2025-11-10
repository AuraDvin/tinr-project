import json


source_file = "Krita-projects/kms.json"
output_file = "ProjectTINR/Content/blobi_animations.json"


def in_range(i:int, min_val:int, max_val:int):
    """Check if i is within range [min_val, max_val] inclusive."""
    return min_val <= i <= max_val


def main():
    with open(source_file) as f:
        data = json.load(f)
    new_data = []
    animations = {
        "idle": {
            "bounds": [0, 5],
            "index": 0
        },
        # "attack": {
        #     "bounds": [27, 33],
        #     "index": 0
        # },
        # "walk": {
        #     "bounds": [38, 45],
        #     "index": 0
        # },
        # "jump": {
        #     "bounds": [45, 61],
        #     "index": 0
        # },
    }

    for animation_name, frames in data.items():
        print(f"Processing: {animation_name}")
        
        for _, frame in enumerate(frames):
            frame_number = frame["filename"]
            
            for anim_name, anim_data in animations.items():
                min_bound, max_bound = anim_data["bounds"]
                if in_range(frame_number, min_bound, max_bound):
                    new_filename = f"{anim_name}"
                    frame["filename"] = new_filename
                    new_data.append(frame)
                    animations[anim_name]["index"] += 1
                    break
    with open(output_file, "w") as f:
        json.dump(new_data, f, indent=2)


if __name__ == "__main__":
    main()