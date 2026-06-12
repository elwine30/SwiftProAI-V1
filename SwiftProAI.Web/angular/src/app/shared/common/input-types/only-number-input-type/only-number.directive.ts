import { Directive, ElementRef, forwardRef, HostListener, Renderer2 } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Directive({
    selector: '[onlyNumber]',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => OnlyNumberDirective),
            multi: true,
        },
    ],
})
export class OnlyNumberDirective implements ControlValueAccessor {
    private onChange: (val: string) => void;
    private onTouched: () => void;
    private value: string;
    private errorElement: HTMLElement;

    constructor(
        private elementRef: ElementRef,
        private renderer: Renderer2,
    ) {}

    @HostListener('keydown', ['$event'])
    onKeyDown(event: KeyboardEvent) {
        // Allow special keys: Backspace, Tab, Enter, Arrow keys, etc.
        if (
            ['Delete', 'Backspace', 'Tab', 'Escape', 'Enter', 'NumpadDecimal', 'Period'].indexOf(event.key) !== -1 ||
            // Allow: Ctrl+A, Command+A
            (event.key === 'a' && (event.ctrlKey || event.metaKey)) ||
            // Allow: Ctrl+C, Command+C
            (event.key === 'c' && (event.ctrlKey || event.metaKey)) ||
            // Allow: Ctrl+V, Command+V
            (event.key === 'v' && (event.ctrlKey || event.metaKey)) ||
            // Allow: Ctrl+X, Command+X
            (event.key === 'x' && (event.ctrlKey || event.metaKey)) ||
            // Allow: home, end, left, right, down, up
            ['Home', 'End', 'ArrowLeft', 'ArrowRight', 'ArrowDown', 'ArrowUp'].indexOf(event.key) !== -1
        ) {
            this.removeErrorElement(); // Remove any existing error message
            return;
        }
        // Stop non numeric values
        if (
            event.shiftKey ||
            !((event.key >= '0' && event.key <= '9') || (event.key >= 'Numpad0' && event.key <= 'Numpad9'))
        ) {
            event.preventDefault();
            this.showError('Please enter only numeric values.');
        } else {
            this.removeErrorElement(); // Remove error message if input is valid
        }
    }

    @HostListener('input', ['$event.target.value'])
    onInput(value: string) {
        const filteredValue: string = this.filterValue(value);
        this.updateTextInput(filteredValue, true);
        this.removeErrorElement(); // Remove any existing error message
    }

    @HostListener('blur')
    onBlur() {
        this.onTouched();
    }

    private updateTextInput(value: string, propagateChange: boolean) {
        this.elementRef.nativeElement.value = value;

        if (propagateChange) {
            this.onChange(value);
        }
        this.value = value;
    }

    private showError(message: string) {
        if (!this.errorElement) {
            this.errorElement = this.renderer.createElement('div');
            this.renderer.addClass(this.errorElement, 'text-danger');
            const text = this.renderer.createText(message);
            this.renderer.appendChild(this.errorElement, text);
            this.renderer.insertBefore(
                this.elementRef.nativeElement.parentNode,
                this.errorElement,
                this.elementRef.nativeElement.nextSibling,
            );
        }
    }

    private removeErrorElement() {
        if (this.errorElement && this.errorElement.parentNode) {
            this.renderer.removeChild(this.errorElement.parentNode, this.errorElement);
            this.errorElement = null;
        }
    }

    // ControlValueAccessor Interface
    registerOnChange(fn: any): void {
        this.onChange = fn;
    }

    registerOnTouched(fn: any): void {
        this.onTouched = fn;
    }

    setDisabledState(isDisabled: boolean): void {
        this.renderer.setProperty(this.elementRef.nativeElement, 'disabled', isDisabled);
    }

    writeValue(value: any): void {
        value = value ? String(value) : '';
        const filteredValue = this.filterValue(value);
        this.updateTextInput(filteredValue, false);
    }

    private filterValue(value: string): string {
        return value.replace(/[^0-9]/g, ''); // Allow only digits
    }
}
