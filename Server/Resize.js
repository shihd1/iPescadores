const sharp = require('sharp');
const { uuid } = require('uuidv4');
const path = require('path');

class Resize {
    constructor(folder) {
        this.folder = folder;
    }
    async save(buffer,fname) {
        const filename = fname.split('.')[0]+"_"+Resize.filename();
        const filepath = this.filepath(filename);

        await sharp(buffer)
            .resize(300, 300, {
                fit: sharp.fit.inside,
                withoutEnlargement: true
            })
            .removeAlpha()
            .toFile(filepath);

        return filename;
    }
    static filename() {
        return `${uuid()}.png`;
    }
    filepath(filename) {
        return path.resolve(`${this.folder}/${filename}`)
    }
}
module.exports = Resize;