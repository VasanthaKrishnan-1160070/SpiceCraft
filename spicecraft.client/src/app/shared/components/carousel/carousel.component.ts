import {Component, CUSTOM_ELEMENTS_SCHEMA, EventEmitter, Input, OnInit, Output} from '@angular/core';
import * as AOS from "aos";
import {TitleComponent} from "../title/title.component";
import {ItemCardComponent} from "../../../feature/item/item-card/item-card.component";
import {MenuItemModel} from "../../../core/model/item/menu-item.model";

@Component({
  selector: 'sc-carousel',
  standalone: true,
  imports: [
    TitleComponent,
    ItemCardComponent
  ],
  templateUrl: './carousel.component.html',
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  styleUrl: './carousel.component.css'
})
export class CarouselComponent implements OnInit {

  @Input() menuItems!: MenuItemModel[];
  @Input() canShowAddToCart = true;
  @Output() addToCart = new EventEmitter<number>();

  ngOnInit() {
    this.initializeAOS();
  }

  initializeAOS() {
    AOS.init({
      // Global settings:
      disable: false, // accepts following values: 'phone', 'tablet', 'mobile', boolean, expression or function
      startEvent: 'DOMContentLoaded', // name of the event dispatched on the document, that AOS should initialize on
      initClassName: 'aos-init', // class applied after initialization
      animatedClassName: 'aos-animate', // class applied on animation
      useClassNames: false, // if true, will add content of `data-aos` as classes on scroll
      disableMutationObserver: false, // disables automatic mutations' detections (advanced)
      debounceDelay: 50, // the delay on debounce used while resizing window (advanced)
      throttleDelay: 99, // the delay on throttle used while scrolling the page (advanced)


      // Settings that can be overridden on per-element basis, by `data-aos-*` attributes:
      offset: 20, // offset (in px) from the original trigger point
      delay: 50, // values from 0 to 3000, with step 50ms
      duration: 1500, // values from 0 to 3000, with step 50ms
      easing: 'ease', // default easing for AOS animations
      once: false, // whether animation should happen only once - while scrolling down
      mirror: false, // whether elements should animate out while scrolling past them
      anchorPlacement: 'top-bottom', // defines which position of the element regarding to window should trigger the animation

    });
  }

  onAddToCart(itemId: number) {
    this.addToCart.emit(itemId);
  }
}
