import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'numberToWords'
})
export class NumberToWordsPipe implements PipeTransform {
    transform(value: any): string {
        if (value && this.isInteger(value)) {
            return this.numToWords(value) + 'RINGGIT';
        } else if (value && this.isFloat(value)) {
            const parts = value.toString().split('.');
            const ringgitPart = this.numToWords(parts[0]);
            // Pad the cents part to ensure it is two digits
            const centsPart = parts[1] ? this.numToWords(parts[1].padEnd(2, '0')) : '';
            return `${ringgitPart}RINGGIT AND ${centsPart}CENTS`;
        } else if (value === 0) return 'ZERO RINGGIT'; 
        return value;
    }
    
    private isInteger(x: any): boolean {
        return x % 1 === 0;
    }
    
    private isFloat(x: any): boolean {
        return !this.isInteger(x) && !isNaN(x);
    }



  private arr = (x: any) => Array.from(x);
  private num = (x: any) => Number(x) || 0;
  private str = (x: any) => String(x);
  private isEmpty = (xs: any[]) => xs.length === 0;
  private take = (n: number) => (xs: any[]) => xs.slice(0, n);
  private drop = (n: number) => (xs: any[]) => xs.slice(n);
  private reverse = (xs: any[]) => xs.slice(0).reverse();
  private comp = (f: Function) => (g: Function) => (x: any) => f(g(x));
  private not = (x: any) => !x;
  private chunk = (n: number) => (xs: any[]) =>
    this.isEmpty(xs) ? [] : [this.take(n)(xs), ...this.chunk(n)(this.drop(n)(xs))];

  private numToWords = (n: any) => {
    let a = [
      '', 'ONE', 'TWO', 'THREE', 'FOUR',
      'FIVE', 'SIX', 'SEVEN', 'EIGHT', 'NINE',
      'TEN', 'ELEVEN', 'TWELVE', 'THIRTEEN', 'FOURTEEN',
      'FIFTEEN', 'SIXTEEN', 'SEVENTEEN', 'EIGHTEEN', 'NINETEEN'
    ];
    let b = [
      '', '', 'TWENTY', 'THIRTY', 'FORTY',
      'FIFTY', 'SIXTY', 'SEVENTY', 'EIGHTY', 'NINETY'
    ];
    let g = [
      '', 'THOUSAND', 'MILLION', 'BILLION', 'TRILLION', 'QUADRILLION',
      'QUINTILLION', 'SEXTILLION', 'SEPTILLION', 'OCTILLION', 'NONILLION'
    ];

    let makeGroup = ([ones, tens, huns]: any[]) => {
      return [
        this.num(huns) === 0 ? '' : a[huns] + ' HUNDRED ',
        this.num(ones) === 0 ? b[tens] : b[tens] && b[tens] + '-' || '',
        a[parseInt(tens + ones)] || a[ones]
      ].join('');
    };

    let thousand = (group: string, i: number) => group === '' ? group : `${group} ${g[i]}`;

    if (typeof n === 'number') return this.numToWords(this.str(n));
    if (n === '0') return 'ZERO ';
    return this.comp(this.chunk(3))(this.reverse)(this.arr(n))
      .map(makeGroup)
      .map(thousand)
      .filter(this.comp(this.not)(this.isEmpty))
      .reverse()
      .join(' ');
  };
}
