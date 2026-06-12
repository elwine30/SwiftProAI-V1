import { Directive, ElementRef, HostListener, Renderer2, OnInit, AfterViewInit } from '@angular/core';

@Directive({
  selector: '[decimal]'
})
export class DecimalFormatDirective implements OnInit {
  constructor(private el: ElementRef, private renderer: Renderer2) { }

  ngOnInit() {
    setTimeout(() => {
      this.formatValue();
    }, 1300);
  }
  // @HostListener('input', ['$event']) onValueChange(event) {
  //   this.formatValue();
  // }
  
  // @HostListener('focus', ['$event']) onFocus(event: Event) {
  //   this.formatValue();
  // }

  @HostListener('blur', ['$event']) onBlur(event: Event) {
    this.formatValue();
  }

  private formatValue() {
    const value = this.el.nativeElement.value;
    const formattedValue = this.formatToTwoDecimalPlaces(value);
    this.renderer.setProperty(this.el.nativeElement, 'value', formattedValue);
  }

  private formatToTwoDecimalPlaces(value: string): string {
    const parsedValue = parseFloat(value);
    if (isNaN(parsedValue)) {
      return '0.00';
    }
    return parsedValue.toFixed(2);
  }
}
