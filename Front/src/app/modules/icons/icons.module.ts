import { NgModule } from '@angular/core';
import { IconXCircle, IconHash, IconDroplet, IconPlay, IconFastForward, IconRewind, IconSkipBack, IconSkipForward, IconPause, IconSave, IconChevronLeft } from 'angular-feather';

const icons = [
  IconXCircle,
  IconHash,
  IconDroplet,
  IconPlay,
  IconFastForward,
  IconRewind,
  IconSkipBack,
  IconSkipForward,
  IconPause,
  IconSave,
  IconChevronLeft
];

@NgModule({
  exports: icons
})
export class IconsModule { }
