INSERT INTO [campaign].[bonus_type] ([type], [display_name], [is_available], [creation_date], [allow_infinite], [allow_percentage], [allow_conversion_rate], [vertical])
     VALUES ('friend-referral', 'Refer a friend to the app', 1, getdate(), 0, 0, 0, NULL);
INSERT INTO [campaign].[bonus_type] ([type], [display_name], [is_available], [creation_date], [allow_infinite], [allow_percentage], [allow_conversion_rate], [vertical])
     VALUES ('emailverified', 'Email verified', 1, getdate(), 0, 0, 0, NULL);
INSERT INTO [campaign].[bonus_type] ([type], [display_name], [is_available], [creation_date], [allow_infinite], [allow_percentage], [allow_conversion_rate], [vertical])
     VALUES ('mvn-purchase', 'Product purchase', 1, getdate(), 0, 1, 0, 'Retail');
INSERT INTO [campaign].[bonus_type] ([type], [display_name], [is_available], [creation_date], [allow_infinite], [allow_percentage], [allow_conversion_rate], [vertical])
     VALUES ('purchase-referral', 'Product purchase (referral)', 1, getdate(), 0, 1, 0, 'Retail');
INSERT INTO [campaign].[bonus_type] ([type], [display_name], [is_available], [creation_date], [allow_infinite], [allow_percentage], [allow_conversion_rate], [vertical])
     VALUES ('signup', 'Sign up', 1, getdate(), 0, 0, 0, NULL);
INSERT INTO [campaign].[bonus_type] ([type], [display_name], [is_available], [creation_date], [allow_infinite], [allow_percentage], [allow_conversion_rate], [vertical])
     VALUES ('estate-lead-referral', 'Real Estate: Agent earns when lead is approved', 1, getdate(), 1, 0, 0, 'RealEstate');
INSERT INTO [campaign].[bonus_type] ([type], [display_name], [is_available], [creation_date], [allow_infinite], [allow_percentage], [allow_conversion_rate], [vertical])
     VALUES ('estate-purchase-referral', 'Real Estate: Agent earns when lead signs SPA', 1, getdate(), 1, 0, 0, 'RealEstate');
INSERT INTO [campaign].[bonus_type] ([type], [display_name], [is_available], [creation_date], [allow_infinite], [allow_percentage], [allow_conversion_rate], [vertical])
     VALUES ('estate-purchase', 'Customer: Signs SPA', 1, getdate(), 1, 0, 0, 'RealEstate');
INSERT INTO [campaign].[bonus_type] ([type], [display_name], [is_available], [creation_date], [allow_infinite], [allow_percentage], [allow_conversion_rate], [vertical])
     VALUES ('estate-otp-referral', 'Real Estate: Agent earns when lead signs OTP', 1, getdate(), 0, 0, 0, 'RealEstate');
INSERT INTO [campaign].[bonus_type] ([type], [display_name], [is_available], [creation_date], [allow_infinite], [allow_percentage], [allow_conversion_rate], [vertical])
     VALUES ('hotel-stay', 'Customer: Stay in Hotel', 1, getdate(), 0, 1, 0, 'Hospitality');
INSERT INTO [campaign].[bonus_type] ([type], [display_name], [is_available], [creation_date], [allow_infinite], [allow_percentage], [allow_conversion_rate], [vertical])
     VALUES ('hotel-stay-referral', 'Referral: Stay in Hotel', 1, getdate(), 0, 1, 0, 'Hospitality');