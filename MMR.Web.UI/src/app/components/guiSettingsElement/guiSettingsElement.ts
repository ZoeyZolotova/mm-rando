import { Component, OnInit, Input, IterableDiffers, ChangeDetectorRef } from '@angular/core';

import { NbDialogService } from '@nebular/theme';
import { NbSelectComponent } from '@nebular/theme/components/select/select.component'

import { GUIGlobal } from '../../providers/GUIGlobal';

import { DialogWindowComponent } from '../../pages/generator/dialogWindow/dialogWindow.component';
import { ConfirmationWindowComponent } from '../../pages/generator/confirmationWindow/confirmationWindow.component';
import { TextInputWindowComponent } from '../../pages/generator/textInputWindow/textInputWindow.component';

//MMR only
import { MMRGuiDetailedConfigWindowComponent } from '../../components/mmr/guiDetailedConfigWindow/guiDetailedConfigWindow.component';
import { MMRItemSelectorWindowComponent } from '../../components/mmr/itemSelectorWindow/itemSelectorWindow.component';

import { GeneratorComponent } from '../../pages/generator/generator.component';

@Component({
  selector: 'gui-settings-element',
  templateUrl: './guiSettingsElement.html',
  styleUrls: ['./guiSettingsElement.scss'],
  standalone: false
})
export class GUISettingsElement implements OnInit {

  //External inputs
  @Input() app: GeneratorComponent = null;
  @Input() section: any = null;
  @Input() sectionSettings: any = null;
  @Input() setting: any = null;
  @Input() itemIndex: number = null;
  @Input() subSetting: any = null;
  @Input() subSettingIndex: number = null;
  @Input() refEl: any = null;
  @Input() tooltipComponent: any = null;

  //Extra overrides
  @Input() assignmentSettingsMap: any = null;
  @Input() assignmentSettingName: string = null;
  @Input() visibilitySettingsMap: any = null;

  @Input() settingDefault: any = null;
  @Input() settingText: string = null;
  @Input() settingTooltip: string = null;
  @Input() skipLabel: boolean = false;

  get dynamicSettingText(): string {
    if (this.setting && this.setting.type === 'Button') {
      switch (this.setting.name) {
        case 'Web.HintPriorities':
          return this.getHintPrioritiesButtonText();
        case 'Web.RandomStartingItemGroups':
          return this.getRandomStartingItemGroupsButtonText();
        default:
          return this.settingText;
      }
    }
    return this.settingText;
  }

  private getHintPrioritiesButtonText(): string {
    const overrideHintPriorities = this.assignmentSettingsMap?.['GameplaySettings.OverrideHintPriorities'] || [];
    const tierCount = Array.isArray(overrideHintPriorities) ? overrideHintPriorities.length : 0;
    if (tierCount === 0) return this.settingText;
    return `Customize Hint Priorities: ${tierCount} ${tierCount === 1 ? 'Sphere' : 'Spheres'}`;
  }

  private getRandomStartingItemGroupsButtonText(): string {
    const randomGroups = this.assignmentSettingsMap?.['GameplaySettings.RandomStartingItemGroups'] || [];
    if (!Array.isArray(randomGroups)) return this.settingText;
    
    const totalAmount = randomGroups.reduce((sum: number, group: any) => sum + (group?.Amount || 0), 0);
    const totalItems = randomGroups.reduce((sum: number, group: any) => sum + (Array.isArray(group?.Items) ? group.Items.length : 0), 0);
    if (totalAmount === 0) return this.settingText;
    return `Random Starting Item Pools: ${totalAmount} / ${totalItems}`;
  }


  // Preset button feedback states
  loadButtonText: string = "Load";
  private loadButtonTimeout: any = null;

  constructor(differs: IterableDiffers, private cd: ChangeDetectorRef, public global: GUIGlobal, private dialogService: NbDialogService) {

  }

  ngOnInit() {
  }

  ngOnChanges(changeRecord) {
  }

  getPresetArray() {
    if (typeof (this.global.generator_presets) == "object")
      return Object.keys(this.global.generator_presets);
    else
      return [];
  }

  loadPreset() {

    let targetPreset = this.global.generator_presets[this.global.generator_settingsMap["Web.presets"]];

    if (targetPreset) {
      if (("isNewPreset" in targetPreset) && targetPreset.isNewPreset == true) {
        this.dialogService.open(DialogWindowComponent, {
          autoFocus: true, closeOnBackdropClick: true, closeOnEsc: true, hasBackdrop: true, hasScroll: false, context: { dialogHeader: "Warning", dialogMessage: "You can not load this preset!" }
        });
      }
      else {

        if (("isDefaultPreset" in targetPreset) && targetPreset.isDefaultPreset == true) { //RESTORE DEFAULTS
          this.global.applyDefaultSettings();
        }
        else {
          this.global.applyDefaultSettings(); //Restore defaults first in case the user loads an old preset that misses settings
          this.global.applySettingsObject(this.global.generator_presets[this.global.generator_settingsMap["Web.presets"]].settings);
        }

        this.app.recheckAllSettings("", false, true);
        this.app.afterSettingChange();

        // Trigger a refresh_gui event to notify the parent component to update
        this.global.globalEmitter.emit({ name: "refresh_gui" });

        // Show success feedback on button
        this.showLoadSuccess();

        //console.log("Preset loaded");
      }
    }
  }

  private showLoadSuccess() {
    if (this.loadButtonTimeout) {
      clearTimeout(this.loadButtonTimeout);
    }

    this.loadButtonText = "Done!";
    this.cd.markForCheck();

    this.loadButtonTimeout = setTimeout(() => {
      this.loadButtonText = "Load";
      this.cd.markForCheck();
      this.cd.detectChanges();
    }, 1000);
  }

  savePreset(refPresetSelect: NbSelectComponent) {

    let targetPreset = this.global.generator_presets[this.global.generator_settingsMap["Web.presets"]];

    if (targetPreset) {

      if ((("isNewPreset" in targetPreset) && targetPreset.isNewPreset == true)) { //NEW PRESET

        this.dialogService.open(TextInputWindowComponent, {
          autoFocus: true, closeOnBackdropClick: true, closeOnEsc: true, hasBackdrop: true, hasScroll: false, context: { dialogHeader: "Create new preset", dialogMessage: "Enter preset name:" }
        }).onClose.subscribe(name => {

          if (name && typeof (name) == "string" && name.trim().length > 0) {

            let trimmedName = name.trim();

            if (trimmedName in this.global.generator_presets) {
              this.dialogService.open(DialogWindowComponent, {
                autoFocus: true, closeOnBackdropClick: true, closeOnEsc: true, hasBackdrop: true, hasScroll: false, context: { dialogHeader: "Error", dialogMessage: "A preset with this name already exists! If you wish to overwrite an existing preset, please select it from the list and hit Save instead." }
              });
            }
            else {
              this.global.generator_presets[trimmedName] = { settings: this.global.createSettingsFileObject(false, true, !this.global.getGlobalVar('electronAvailable')) };
              this.global.generator_settingsMap["Web.presets"] = trimmedName;
              this.global.saveCurrentPresetsToFile();

              this.cd.markForCheck();
              this.cd.detectChanges();

              refPresetSelect.selected = trimmedName;

              //console.log("Preset created");
            }
          }
        });
      }
      else if (("isDefaultPreset" in targetPreset) && targetPreset.isDefaultPreset == true) { //DEFAULT
        this.dialogService.open(DialogWindowComponent, {
          autoFocus: true, closeOnBackdropClick: true, closeOnEsc: true, hasBackdrop: true, hasScroll: false, context: { dialogHeader: "Warning", dialogMessage: "System presets can not be overwritten!" }
        });
      }
      else if (("isProtectedPreset" in targetPreset) && targetPreset.isProtectedPreset == true) { //BUILT IN
        this.dialogService.open(DialogWindowComponent, {
          autoFocus: true, closeOnBackdropClick: true, closeOnEsc: true, hasBackdrop: true, hasScroll: false, context: { dialogHeader: "Warning", dialogMessage: "Built in presets are protected and can not be overwritten!" }
        });
      }
      else { //USER PRESETS
        this.dialogService.open(ConfirmationWindowComponent, {
          autoFocus: true, closeOnBackdropClick: true, closeOnEsc: true, hasBackdrop: true, hasScroll: false, context: { dialogHeader: "Confirm?", dialogMessage: "Do you want to overwrite the preset '" + this.global.generator_settingsMap["Web.presets"] + "' ?" }
        }).onClose.subscribe(confirmed => {

          if (confirmed) {
            this.global.generator_presets[this.global.generator_settingsMap["Web.presets"]] = { settings: this.global.createSettingsFileObject(false, true, !this.global.getGlobalVar('electronAvailable')) };
            this.global.saveCurrentPresetsToFile();

            console.log("Preset overwritten");
          }
        });
      }
    }
  }

  deletePreset() {

    let targetPreset = this.global.generator_presets[this.global.generator_settingsMap["Web.presets"]];

    if (targetPreset) {
      if ((("isNewPreset" in targetPreset) && targetPreset.isNewPreset == true) || (("isDefaultPreset" in targetPreset) && targetPreset.isDefaultPreset == true)) {
        this.dialogService.open(DialogWindowComponent, {
          autoFocus: true, closeOnBackdropClick: true, closeOnEsc: true, hasBackdrop: true, hasScroll: false, context: { dialogHeader: "Warning", dialogMessage: "System presets can not be deleted!" }
        });
      }
      else if (("isProtectedPreset" in targetPreset) && targetPreset.isProtectedPreset == true) {
        this.dialogService.open(DialogWindowComponent, {
          autoFocus: true, closeOnBackdropClick: true, closeOnEsc: true, hasBackdrop: true, hasScroll: false, context: { dialogHeader: "Warning", dialogMessage: "Built in presets are protected and can not be deleted!" }
        });
      }
      else {
        this.dialogService.open(ConfirmationWindowComponent, {
          autoFocus: true, closeOnBackdropClick: true, closeOnEsc: true, hasBackdrop: true, hasScroll: false, context: { dialogHeader: "Confirm?", dialogMessage: "Do you really want to delete the preset '" + this.global.generator_settingsMap["Web.presets"] + "' ?" }
        }).onClose.subscribe(confirmed => {

          if (confirmed) {
            delete this.global.generator_presets[this.global.generator_settingsMap["Web.presets"]];
            this.global.generator_settingsMap["Web.presets"] = "[New Preset]";
            this.global.saveCurrentPresetsToFile();

            this.cd.markForCheck();
            this.cd.detectChanges();

            //console.log("Preset deleted");
          }
        });
      }
    }
  }

  exportPreset() {

    let presetName = this.global.generator_settingsMap["Web.presets"];
    let targetPreset = this.global.generator_presets[presetName];

    if (!targetPreset) {
      return;
    }

    if ((("isNewPreset" in targetPreset) && targetPreset.isNewPreset == true) || (("isDefaultPreset" in targetPreset) && targetPreset.isDefaultPreset == true)) {

      this.dialogService.open(DialogWindowComponent, {
        autoFocus: true, closeOnBackdropClick: true, closeOnEsc: true, hasBackdrop: true, hasScroll: false, context: { dialogHeader: "Warning", dialogMessage: "System presets can not be exported!" }
      });

      return;
    }

    let settingsToExport = targetPreset.settings;
    let exportObject = {
      name: presetName,
      settings: settingsToExport
    };

    let jsonString = JSON.stringify(exportObject, null, 2);
    let blob = new Blob([jsonString], { type: 'application/json' });
    let url = URL.createObjectURL(blob);

    let a = document.createElement('a');
    a.href = url;
    a.download = presetName.replace(/[^a-z0-9]/gi, '_') + '.json';
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
    URL.revokeObjectURL(url);
  }

  importPresets(fileInput: HTMLInputElement) {
    fileInput.click();
  }

  async onImportFilesSelected(event: Event) {

    let input = event.target as HTMLInputElement;
    let files = input.files;

    if (!files || files.length === 0) {
      return;
    }

    let importedCount = 0;
    let skippedCount = 0;
    let errorCount = 0;
    let errorMessages: string[] = [];

    for (let i = 0; i < files.length; i++) {
      let file = files[i];

      try {
        let fileContent = await this.global.readFileIntoMemoryWeb(file, false);

        if (!fileContent || fileContent.length < 1) {
          errorCount++;
          errorMessages.push(`${file.name}: File is empty`);
          continue;
        }

        if (fileContent.length > (1000 * 1000)) {
          errorCount++;
          errorMessages.push(`${file.name}: File is too large (max 1MB)`);
          continue;
        }

        let parsedContent;
        try {
          parsedContent = JSON.parse(fileContent);
        }
        catch (parseErr) {
          errorCount++;
          errorMessages.push(`${file.name}: Invalid JSON syntax`);
          continue;
        }

        if (!parsedContent || typeof parsedContent !== 'object') {
          errorCount++;
          errorMessages.push(`${file.name}: Invalid preset format`);
          continue;
        }

        let presetName: string;
        let presetSettings: any;

        if ('name' in parsedContent && 'settings' in parsedContent && typeof parsedContent.settings === 'object') {
          presetName = parsedContent.name;
          presetSettings = parsedContent.settings;
        }
        else if (Object.keys(parsedContent).length > 0) {
          presetName = file.name.replace(/\.json$/i, '');
          presetSettings = parsedContent;
        }
        else {
          errorCount++;
          errorMessages.push(`${file.name}: Empty or invalid preset`);
          continue;
        }

        if (!presetName || presetName.trim().length === 0) {
          presetName = file.name.replace(/\.json$/i, '');
        }
        presetName = presetName.trim();

        let existingPreset = this.global.generator_presets[presetName];
        if (existingPreset) {
          if (("isNewPreset" in existingPreset) || ("isDefaultPreset" in existingPreset) || ("isProtectedPreset" in existingPreset)) {
            let suffix = 1;
            let newName = presetName + "_" + suffix;
            while (this.global.generator_presets[newName]) {
              suffix++;
              newName = presetName + "_" + suffix;
            }
            presetName = newName;
          }
        }

        this.global.generator_presets[presetName] = { settings: presetSettings };
        importedCount++;

      }
      catch (ex) {
        errorCount++;
        errorMessages.push(`${file.name}: ${ex.error || 'Unknown error'}`);
      }
    }

    input.value = '';

    if (importedCount > 0) {
      this.global.saveCurrentPresetsToFile();
      this.cd.markForCheck();
      this.cd.detectChanges();
    }

    let message = '';
    if (importedCount > 0) {
      message += `Successfully imported ${importedCount} preset${importedCount > 1 ? 's' : ''}.`;
    }
    if (skippedCount > 0) {
      message += ` Skipped ${skippedCount} duplicate${skippedCount > 1 ? 's' : ''}.`;
    }
    if (errorCount > 0) {
      message += ` Failed to import ${errorCount} file${errorCount > 1 ? 's' : ''}.`;
      if (errorMessages.length > 0) {
        message += '\n\nErrors:\n' + errorMessages.slice(0, 5).join('\n');
        if (errorMessages.length > 5) {
          message += `\n... and ${errorMessages.length - 5} more`;
        }
      }
    }

    if (message.length > 0) {
      this.dialogService.open(DialogWindowComponent, {
        autoFocus: true, closeOnBackdropClick: true, closeOnEsc: true, hasBackdrop: true, hasScroll: false,
        context: { dialogHeader: errorCount > 0 && importedCount === 0 ? "Error" : "Import Complete", dialogMessage: message }
      });
    }

    console.log(`Preset import complete: ${importedCount} imported, ${skippedCount} skipped, ${errorCount} errors`);
  }

  //Button settings type listeners
  openOutputDir() { //Electron only

    var path = "";

    if (!this.global.generator_settingsMap["output_dir"] || this.global.generator_settingsMap["output_dir"].length < 1)
      path = "Output";
    else
      path = this.global.generator_settingsMap["output_dir"];

    this.global.createAndOpenPath(path).then(() => {
      console.log("Output dir opened");
    }).catch(err => {
      console.error("Error:", err);

      if (err.message.includes("no such file or directory")) {
        this.dialogService.open(DialogWindowComponent, {
          autoFocus: true, closeOnBackdropClick: true, closeOnEsc: true, hasBackdrop: true, hasScroll: false, context: { dialogHeader: "Error", dialogMessage: "The specified output directory does not exist!" }
        });
      }
      else {
        this.dialogService.open(DialogWindowComponent, {
          autoFocus: true, closeOnBackdropClick: true, closeOnEsc: true, hasBackdrop: true, hasScroll: false, context: { dialogHeader: "Error", dialogMessage: err }
        });
      }
    });
  }

  openPythonDir() { //Electron only

    this.global.createAndOpenPath("").then(() => {
      console.log("Python dir opened");
    }).catch(err => {
      console.error("Error:", err);

      this.dialogService.open(DialogWindowComponent, {
        autoFocus: true, closeOnBackdropClick: true, closeOnEsc: true, hasBackdrop: true, hasScroll: false, context: { dialogHeader: "Error", dialogMessage: err }
      });
    });
  }

  //MMR only
  openRandomStartingItemsWindow(setting: any, assignmentSettingsMap: any, assignmentSettingName: string, settingDefault: any, settingText: string, settingTooltip: string) {
    this.setAppContainerDimensions();
    const dialogRef = this.dialogService.open(MMRGuiDetailedConfigWindowComponent, {
      autoFocus: true, 
      closeOnBackdropClick: true, 
      closeOnEsc: true, 
      hasBackdrop: true, 
      hasScroll: false,
      context: { 
        dialogHeader: settingText, 
        setting, 
        assignmentSettingsMap, 
        assignmentSettingName, 
        sectionSettings: this.sectionSettings, 
        configMode: 'randomStartingItemGroups'
      }
    });
    this.resizeDialogToAppContainer('.mmrGuiDetailedConfig-window', 0.78, 0.78);
    dialogRef.onClose.subscribe(result => {
      if (result) {
        this.app.afterSettingChange();
      }
    });
  }

  openHintPrioritiesWindow(setting: any, assignmentSettingsMap: any, assignmentSettingName: string, settingDefault: any, settingText: string, settingTooltip: string, event?: any) {
    // Measure the app container and set custom properties for dialog sizing
    this.setAppContainerDimensions();

    //Backup existing settings
    let overrideHintPriorities = JSON.parse(JSON.stringify(this.assignmentSettingsMap['GameplaySettings.OverrideHintPriorities'] || []));
    let overrideImportanceIndicatorTiers = JSON.parse(JSON.stringify(this.assignmentSettingsMap['GameplaySettings.OverrideImportanceIndicatorTiers'] || []));
    let overrideHintItemCaps = JSON.parse(JSON.stringify(this.assignmentSettingsMap['GameplaySettings.OverrideHintItemCaps'] || []));

    const dialogRef = this.dialogService.open(MMRGuiDetailedConfigWindowComponent, {
      autoFocus: true, 
      closeOnBackdropClick: true, 
      closeOnEsc: true, 
      hasBackdrop: true, 
      hasScroll: false,
      context: { 
        dialogHeader: settingText, 
        setting, 
        assignmentSettingsMap, 
        assignmentSettingName,
        sectionSettings: this.sectionSettings,
        configMode: 'hintPriorities'
      }
    });

    // Set dialog size based on app container immediately
    this.resizeDialogToAppContainer('.mmrGuiDetailedConfig-window', 0.78, 0.78);

    dialogRef.onClose.subscribe(result => {

      if (result) {
        //Save
        this.app.afterSettingChange();
      }
      else {
        //Revert values
        this.assignmentSettingsMap['GameplaySettings.OverrideHintPriorities'] = overrideHintPriorities;
        this.assignmentSettingsMap['GameplaySettings.OverrideImportanceIndicatorTiers'] = overrideImportanceIndicatorTiers;
        this.assignmentSettingsMap['GameplaySettings.OverrideHintItemCaps'] = overrideHintItemCaps;
      }
    });
  }

  private setAppContainerDimensions() {
    // Find the app container (the main card or content area)
    const appContainer = document.querySelector('.pageContainer') || 
                       document.querySelector('nb-card') || 
                       document.querySelector('#generator');
    
    if (appContainer) {
      const rect = appContainer.getBoundingClientRect();
      const appWidth = rect.width;
      const appHeight = rect.height;
      
      // Set CSS custom properties for dialog sizing
      document.documentElement.style.setProperty('--app-width', `${appWidth}px`);
      document.documentElement.style.setProperty('--app-height', `${appHeight}px`);
    } else {
      // Fallback to viewport dimensions if app container not found
      document.documentElement.style.setProperty('--app-width', `${window.innerWidth}px`);
      document.documentElement.style.setProperty('--app-height', `${window.innerHeight}px`);
    }
  }

  private resizeDialogToAppContainer(selector: string, widthRatio: number, heightRatio: number) {
    const appContainer = document.querySelector('.pageContainer') || 
                       document.querySelector('nb-card') || 
                       document.querySelector('#generator');
    
    if (appContainer) {
      const appRect = appContainer.getBoundingClientRect();
      const dialogElement = document.querySelector(selector) as HTMLElement;
      
      if (dialogElement) {
        const effectiveWidthRatio = window.innerWidth < 750 ? 0.99 : widthRatio;
        const targetWidth = appRect.width * effectiveWidthRatio;
        
        let targetHeight: number;
        if (window.innerWidth <= 800) {
          targetHeight = window.innerHeight * 0.75;
        } else {
          targetHeight = appRect.height * heightRatio;
        }
        
        const maxAllowedHeight = window.innerHeight * 0.80;
        targetHeight = Math.min(targetHeight, maxAllowedHeight);
        
        const centerX = appRect.left + (appRect.width / 2);
        let centerY = window.innerHeight / 2;
        
        if (window.innerWidth > 1000) {
          centerY = window.innerHeight / 2 + 40; // add 40 to account for top bar
        }

        dialogElement.style.width = `${targetWidth}px`;
        dialogElement.style.height = `${targetHeight}px`;
        dialogElement.style.maxWidth = `${targetWidth}px`;
        dialogElement.style.maxHeight = `${targetHeight}px`;
        
        dialogElement.style.position = 'fixed';
        dialogElement.style.left = `${centerX - (targetWidth / 2)}px`;
        dialogElement.style.top = `${centerY - (targetHeight / 2)}px`;
        dialogElement.style.transform = 'none';
        dialogElement.style.zIndex = '1000';
      } else {
        const alternativeSelectors = [
          '.mmrItemSelector-window',
          '.mmrGuiDetailedConfig-window',
          '[class*="ItemSelector"]',
          '[class*="HintPriorities"]'
        ];
        
        for (const altSelector of alternativeSelectors) {
          const altElement = document.querySelector(altSelector);
        }
      }
    }
  }

  generateDictSetting(mainSetting, keySetting) {

    let newKeySetting = { ...keySetting };
    newKeySetting.type = mainSetting.inner_type;

    return newKeySetting;
  }

  evaluateLinkedDictSettingVisibility(dictSetting: any, allDictSettings: any, currSelectedDictKey: string, visibilitySettingsMap: any) {

    //Note: Only supports simple complexity
    let visibilityObj = {};

    //By default every dict sub setting is enabled
    visibilityObj[dictSetting.name] = true;

    //Immediately return disabled if the dict setting is disabled globally
    if (!visibilitySettingsMap[dictSetting.name]) {
      visibilityObj[dictSetting.name] = false;
      return visibilityObj;
    }

    //See if any other dict setting disables this setting with the current key selected
    for (let setting of allDictSettings) {

      if (setting.type == "Checkbutton" && setting.options) {

        for (let option of setting.options) {

          if ("controls_visibility_setting" in option) {
            let settingsList = option.controls_visibility_setting.split(",");

            if (settingsList.indexOf(dictSetting.name) != -1) {

              let currentValue = this.global.generator_settingsMap[setting.name][currSelectedDictKey];

              if (currentValue === option.name) {
                //console.log("In active key:", currSelectedDictKey, dictSetting.name, "is actively disabled by:", setting.name, "being set to:", option.name);
                visibilityObj[dictSetting.name] = false;
                return visibilityObj;
              }
            }
          }
        }
      }
    }

    return visibilityObj;
  }

  onFileInputChange(event: any) {
    // Handle the input change and call the original afterSettingChange method
    this.app.afterSettingChange();
  }
}
