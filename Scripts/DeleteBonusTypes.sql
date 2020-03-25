USE [CampaignContext]
GO

DELETE FROM [campaign].[condition]
      WHERE [bonus_type] in ('property-purchase-commission-two','estate-purchase-referral', 'commission-two-referral')
GO

USE [CampaignContext]
GO

DELETE FROM [campaign].[bonus_type]
      WHERE [bonus_type].[type] in ('property-purchase-commission-two','estate-purchase-referral', 'commission-two-referral')
GO


