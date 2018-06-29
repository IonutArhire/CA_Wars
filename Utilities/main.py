from moviepy.editor import ImageSequenceClip
from PIL import Image
import sys


def gen_filenames(images_dir, imgs_count):
    return [images_dir + '\\\\' + str(i) + '.png' for i in range(0, imgs_count)]
    
def crop_images(images):
    for unprocessed_image in images:
        print(unprocessed_image)

        image=Image.open(unprocessed_image)

        imageBox = image.getbbox()
        cropped=image.crop(imageBox)
        cropped.save(unprocessed_image)

def main(args):
    images_dir = args[1]
    imgs_count = int(args[2])
    fps = int(args[3])
    format = args[4]

    images = gen_filenames(images_dir, imgs_count)
    crop_images(images)

    clip = ImageSequenceClip(images, fps=fps)
    
    if format == "gif":
        clip.write_gif('giffer' + '.gif')
    else:
        clip.write_videofile('movie' + '.mp4')

if __name__ == '__main__':
    main(sys.argv)