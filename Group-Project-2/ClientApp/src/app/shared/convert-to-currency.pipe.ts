import { Pipe, PipeTransform } from "@angular/core";

@Pipe({
  name: 'convertToCurrency'
})

export class ConvertToCurrency implements PipeTransform {
  transform(value: number, character: string): string {
    return character + ' ' + value.toString();
  }
}
