import * as React from "react";
import { SimpleForm, PasswordInput, TextInput, required } from 'react-admin';
import { Create } from 'react-admin';
import { RadioButtonGroupInput } from 'react-admin';
import { FormDataConsumer } from 'react-admin';

const types=[
    { id: 'strava', name: 'Strava', full_name: 'Strava' },
    { id: 'garmin', name: 'Garmin', full_name: 'Garmin Connect' }
];

export const StravaConectionCreate = props => (
    <Create {...props}>
        <SimpleForm>
            <RadioButtonGroupInput source="connectionType"  choices={types} optionText="full_name" optionValue="id"/>    
            <FormDataConsumer>
                {({ formData, ...rest }) => formData.connectionType === 'garmin' &&
                      <TextInput source="id" label="Username" validate={required()} {...rest} />
                 }
             </FormDataConsumer>
             <FormDataConsumer>
                {({ formData, ...rest }) => formData.connectionType === 'garmin' &&
                      <PasswordInput source="password" label="Password" validate={required()} {...rest} />
                 }
             </FormDataConsumer>
             <FormDataConsumer>
                 {({ formData, ...rest }) => formData.connectionType === 'strava' &&
                      <TextInput source="id" label="ClientId" validate={required()} {...rest} />
                 }
             </FormDataConsumer>
             <FormDataConsumer>
                {({ formData, ...rest }) => formData.connectionType === 'strava' &&
                      <PasswordInput source="token" label="Token" validate={required()} {...rest} />
                 }
             </FormDataConsumer>
             <FormDataConsumer>
                {({ formData, ...rest }) => formData.connectionType === 'strava' &&
                      <PasswordInput source="refreshToken" label="Refresh token" validate={required()} {...rest} />
                 }
             </FormDataConsumer>
        </SimpleForm>
    </Create>
);