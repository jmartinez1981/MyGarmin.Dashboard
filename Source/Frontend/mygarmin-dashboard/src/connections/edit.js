import * as React from "react";
import { SimpleForm, FormDataConsumer, PasswordInput, TextInput, required } from 'react-admin';
import { Edit } from 'react-admin';

export const ConnectionEdit = (props) => (
    <Edit {...props}>
        <SimpleForm>
            <TextInput disabled label="Id" source="id" />
            <FormDataConsumer>
                {({ formData, ...rest }) => formData.connectionType === 'garmin' &&
                      <PasswordInput source="password" label="Password" validate={required()} {...rest} />
                 }
             </FormDataConsumer>
             <FormDataConsumer>
                {({ formData, ...rest }) => formData.connectionType === 'strava' &&
                      <PasswordInput source="secret" label="Client secret" validate={required()} {...rest} />
                 }
             </FormDataConsumer>
        </SimpleForm>
    </Edit>
);