import { Directive, ElementRef, HostListener } from '@angular/core';

@Directive({
  selector: '[amount]'
})
export class AmountInputDirective {
  private regex: RegExp = new RegExp(/^\d*\.?\d{0,2}$/g);
  private specialKeys: Array<string> = ['Backspace', 'Tab', 'End', 'Home', 'ArrowLeft', 'ArrowRight', 'ArrowUp', 'ArrowDown'];

  constructor(private el: ElementRef) { }

  @HostListener('input', ['$event'])
  onKeyDown(event: Event) {
    // Allow: Delete, Backspace, Tab, Escape, Enter, etc
    if (this.specialKeys.indexOf(event.type) !== -1) {
      return;
    }

    // Validate if the new value has more than two decimal places
    let inputElement = event.target as HTMLInputElement
    let newvalue: string = inputElement.value;
    if (newvalue && !String(newvalue).match(this.regex)) {
      inputElement.value = newvalue.slice(0, -1); // Format the value to fit within the two decimal places
    }
  }

  @HostListener('paste', ['$event'])
  onPaste(event: ClipboardEvent) {
    const clipboardData = event.clipboardData || window['clipboardData'];
    let pastedInput = clipboardData.getData('text');

    if (!pastedInput.match(this.regex)) {
      event.preventDefault();
    }
  }
}
