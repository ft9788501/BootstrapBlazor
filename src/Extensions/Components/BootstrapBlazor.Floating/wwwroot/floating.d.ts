import ' rollup-plugin-inject-process-env';
import { Placement, Strategy } from '@floating-ui/dom';
declare type FloatingRefType = 'cssSelecter' | 'elementReference' | 'idComponentBase' | 'mouseEventArgs';
interface FloatingConfig {
    show: boolean;
    useVirtualElement: boolean;
    referenceType: FloatingRefType;
    initialState: string;
    strategy: Strategy;
    placement: Placement;
    mainAxis: number;
    shiftPadding: number;
    useArrow: boolean;
    arrowOffset: number;
    autoUpdate: boolean;
    refElementId: string | null;
    clientX: number;
    clientY: number;
}
export declare function computeFloating(interop: any, reference: any, floating: HTMLElement, config: FloatingConfig): void;
export declare function cleanupFloating(floating: HTMLElement): void;
export {};
