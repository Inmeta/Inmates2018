import * as React from 'react';
import * as ReactDom from 'react-dom';
import { Version } from '@microsoft/sp-core-library';
import {
  BaseClientSideWebPart,
  IPropertyPaneConfiguration,
  PropertyPaneTextField
} from '@microsoft/sp-webpart-base';

import * as strings from 'WebTemplateGeneratorWebPartStrings';
import WebTemplateGenerator from './components/WebTemplateGenerator';
import { IWebTemplateGeneratorProps } from './components/IWebTemplateGeneratorProps';

export interface IWebTemplateGeneratorWebPartProps {
  description: string;
}

export default class WebTemplateGeneratorWebPart extends BaseClientSideWebPart<IWebTemplateGeneratorWebPartProps> {

  public render(): void {
    const element: React.ReactElement<IWebTemplateGeneratorProps > = React.createElement(
      WebTemplateGenerator,
      {
        description: this.properties.description
      }
    );

    ReactDom.render(element, this.domElement);
  }

  protected get dataVersion(): Version {
    return Version.parse('1.0');
  }

  protected getPropertyPaneConfiguration(): IPropertyPaneConfiguration {
    return {
      pages: [
        {
          header: {
            description: strings.PropertyPaneDescription
          },
          groups: [
            {
              groupName: strings.BasicGroupName,
              groupFields: [
                PropertyPaneTextField('description', {
                  label: strings.DescriptionFieldLabel
                })
              ]
            }
          ]
        }
      ]
    };
  }
}
