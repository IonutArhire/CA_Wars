import { NgModule } from '@angular/core';
import { IconXCircle, IconHash, IconDroplet, IconPlay, IconFastForward, IconRewind, IconSkipBack, IconSkipForward, IconPause, IconSave, IconChevronLeft, IconAlertCircle, IconThumbsUp, IconShield, IconNavigation2 } from 'angular-feather';

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
  IconChevronLeft,
  IconAlertCircle,
  IconThumbsUp,
  IconShield,
  IconNavigation2
];

@NgModule({
  exports: icons
})
export class IconsModule { }
