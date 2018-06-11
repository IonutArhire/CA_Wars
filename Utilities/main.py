from moviepy.editor import ImageSequenceClip
import os
import sys

def gen_filenames(images_dir, imgs_count):
    return [os.path.join(images_dir, str(i) + '.png') for i in range(0, imgs_count)]

def main(args):
    images_dir = args[1]
    imgs_count = int(args[2])
    fps = int(args[3])

    images = gen_filenames(images_dir, imgs_count)

    video = ImageSequenceClip(images, fps=fps)
    video.write_videofile('background.mp4')

if __name__ == '__main__':
    main(sys.argv)