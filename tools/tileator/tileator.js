let fs = require('fs');
let PNG = require('pngjs').PNG

// 0 = floor, 1 = wall, 2 = water

let TYPE_NAME = [
    'floor', 'wall', 'water'
];

let TILE_TYPES = [
    1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 0, 0, 0, 0, 0,
    0, 0, 0, 2, 2, 2, 2, 0, 2, 0, 0, 0, 2, 2, 2, 0, 0, 0, 0, 0,
    0, 0, 0, 2, 0, 0, 2, 0, 2, 0, 0, 0, 2, 2, 2, 0, 0, 0, 0, 0,
    0, 0, 0, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    0, 0, 0, 2, 2, 2, 2, 0, 0, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0,
    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
]

let type_count = [0, 0, 0];

let in_png = PNG.sync.read(fs.readFileSync('../../art_src/sewers_mockup.png'));
// console.log(in_png);
// process.exit();

var out_png = new PNG({
    width: 32,
    height: 32,
    filterType: -1
});

let is_first_blank = true;
for (let tile_y = 0; tile_y < 20; ++tile_y)
{
    for (let tile_x = 0; tile_x < 20; ++tile_x)
    {
        let tile_idx = tile_y * 20 + tile_x;
        let tile_type = TILE_TYPES[tile_idx];
        let type_name = TYPE_NAME[tile_type];
        let type_id = type_count[tile_type]++;

        let is_blank = true;

        for (let fine_y = 0; fine_y < 32; ++fine_y)
        {
            for (let fine_x = 0; fine_x < 32; ++fine_x)
            {
                let x = tile_x * 32 + fine_x;
                let y = tile_y * 32 + fine_y;
                let k = (y * 640 + x) * 4;
                let l = (fine_y * 32 + fine_x) * 4;

                let r = in_png.data[k + 0];
                let g = in_png.data[k + 1];
                let b = in_png.data[k + 2];
                let a = in_png.data[k + 3];

                out_png.data[l + 0] = r;
                out_png.data[l + 1] = g;
                out_png.data[l + 2] = b;
                out_png.data[l + 3] = a;

                if (r != 8 && g != 6 && b != 23) is_blank = false;
            }
        }

        if (is_blank && !is_first_blank) continue;
        if (is_blank) is_first_blank = false;

        let out_filename = `../../content/sewer_tiles/${type_name}_${type_id.toString()}.png`;

        let buffer = PNG.sync.write(out_png, {colorType: 6})
        fs.writeFileSync(out_filename, buffer)
    }
}
