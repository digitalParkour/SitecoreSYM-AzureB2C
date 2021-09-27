<%@ control language="C#" %>
<%@ register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI.Editor" tagprefix="tools" %>
<%@ register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<%@ register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI.Widgets" tagprefix="widgets" %>
<%@ register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI.Dialogs" tagprefix="dialogs" %>


<script type="text/javascript" src="/sitecore/shell/Controls/Lib/jQuery/jquery-1.12.4.min.js"></script>
<style type="text/css">
	.LinkManager .reControlCell input[type="radio"],
	.LinkManager .reControlCell input[type="checkbox"] {
		width: 20px;
	}

	html .InsertInternalLink {
		background-position: -3697px center !important;
	}
</style>
<table cellpadding="0" cellspacing="0" class="reDialog LinkManager NoMarginDialog" style="width: 400px;">
	<tr>
		<td class="reTopcell">
			<telerik:radtabstrip showbaseline="true" id="LinkManagerTab" runat="server" selectedindex="0" multipageid="dialogMultiPage">
				<Tabs>
					<telerik:RadTab Text="HyperlinkTab" Value="HyperlinkTab">
					</telerik:RadTab>
					<telerik:RadTab Text="AnchorTab" Value="AnchorTab">
					</telerik:RadTab>
					<telerik:RadTab Text="EmailTab" Value="EmailTab">
					</telerik:RadTab>
				    <telerik:RadTab Text="Analytics Event" Value="AnalyticsEventTab">
				    </telerik:RadTab>
				</Tabs>
			</telerik:radtabstrip>
		</td>
	</tr>
	<tr>
		<td class="reMiddlecell" style="height: 220px; vertical-align: top;">
			<telerik:radmultipage id="dialogMultiPage" runat="server" selectedindex="0">
				<telerik:RadPageView ID="hyperlinkFieldset" runat="server">
					<table border="0" cellpadding="0" cellspacing="0" class="reControlsLayout">
						<asp:PlaceHolder ID="documentCallerRow" runat="server">
							<tr>
								<td class="reLabelCell">
									<label for="LinkURL" class="reDialogLabel">
										<span>
											<script type="text/javascript">document.write(localization["LinkUrl"]);</script>
										</span>
									</label>
								</td>
								<td class="reControlCell">
									<table border="0" cellpadding="" cellspacing="0">
										<tr>
											<td>
												<input type="text" id="LinkURL" style="width: 212px;" />
											</td>
											<td style="padding-left: 4px;">
												<tools:StandardButton runat="server" ToolName="DocumentManager" ID="DocumentManagerCaller" />
											</td>
										    <td style="padding-left: 4px;">
										        <tools:StandardButton runat="server" ToolName="InsertInternalLink" id="InsertInternalLink" />
										    </td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td class="reLabelCell">
									<label for="OverlaySource" class="reDialogLabel">
										<span>Open In Overlay</span>
									</label>
								</td>
								<td class="reControlCell">
									<input type="checkbox" id="OverlaySource" />
								</td>
							</tr>
						</asp:PlaceHolder>
						<tr id="texTextBoxParentNode">
							<td class="reLabelCell">
								<label for="LinkText" class="reDialogLabel">
									<span>
										<script type="text/javascript">document.write(localization["LinkText"]);</script>
									</span>
								</label>
							</td>
							<td class="reControlCell">
								<input type="text" id="LinkText" />
							</td>
						</tr>
						<tr>
							<td class="reLabelCell">
								<label for="LinkTargetCombo" class="reDialogLabel">
									<span>
										<script type="text/javascript">document.write(localization["LinkTarget"]);</script>
									</span>
								</label>
							</td>
							<td class="reControlCell">
								<select id="LinkTargetCombo">
									<optgroup label="PresetTargets">
										<option value="_none">None</option>
										<option value="_self">TargetSelf</option>
										<option value="_blank">TargetBlank</option>
										<option value="_parent">TargetParent</option>
										<option value="_top">TargetTop</option>
										<option value="_search">TargetSearch</option>
										<option value="_media">TargetMedia</option>
									</optgroup>
									<optgroup label="CustomTargets">
										<option value="_custom">AddCustomTarget</option>
									</optgroup>
								</select>
							</td>
						</tr>
						<asp:PlaceHolder ID="existingAnchorRow" runat="server">
							<tr>
								<td class="reLabelCell">
									<label for="ExistingAnchor" class="reDialogLabel">
										<span>
											<script type="text/javascript">document.write(localization["ExistingAnchor"]);</script>
										</span>
									</label>
								</td>
								<td class="reControlCell">
									<select id="ExistingAnchor">
										<option selected="selected">None</option>
									</select>
								</td>
							</tr>
						</asp:PlaceHolder>
						<tr>
							<td class="reLabelCell">
								<label for="LinkTooltip" class="reDialogLabel">
									<span>
										<script type="text/javascript">document.write(localization["LinkTooltip"]);</script>
									</span>
								</label>
							</td>
							<td class="reControlCell">
								<input type="text" id="LinkTooltip" />
							</td>
						</tr>
						<tr>
							<td class="reLabelCell">
								<label for="LinkCssClass" class="reDialogLabel">
									<span>
										<script type="text/javascript">document.write(localization["CssClass"]);</script>
									</span>
								</label>
							</td>
							<td class="reControlCell">
								<tools:ApplyClassDropDown ID="LinkCssClass" runat="server" />
							</td>
						</tr>
					</table>
				</telerik:RadPageView>
				<telerik:RadPageView ID="anchorFieldset" runat="server">
					<table border="0" cellpadding="0" cellspacing="0" class="reControlsLayout">
						<tr>
							<td class="reLabelCell">
								<label for="AnchorName" class="reDialogLabel">
									<span>
										<script type="text/javascript">document.write(localization["LinkName"]);</script>
									</span>
								</label>
							</td>
							<td class="reControlCell">
								<input type="text" id="AnchorName" />
							</td>
						</tr>
					</table>
				</telerik:RadPageView>
				<telerik:RadPageView ID="emailFieldset" runat="server">
					<table border="0" cellpadding="0" cellspacing="0" class="reControlsLayout">
						<tr>
							<td class="reLabelCell">
								<label for="EmailAddress" class="reDialogLabel">
									<span>
										<script type="text/javascript">document.write(localization["LinkAddress"]);</script>
									</span>
								</label>
							</td>
							<td class="reControlCell">
								<input type="text" id="EmailAddress" />
							</td>
						</tr>
						<tr id="emailTextBoxParentNode">
							<td class="reLabelCell">
								<label for="EmailLinkText" class="reDialogLabel">
									<span>
										<script type="text/javascript">document.write(localization["LinkText"]);</script>
									</span>
								</label>
							</td>
							<td class="reControlCell">
								<input type="text" id="EmailLinkText" />
							</td>
						</tr>
						<tr>
							<td class="reLabelCell">
								<label for="EmailSubject" class="reDialogLabel">
									<span>
										<script type="text/javascript">document.write(localization["LinkSubject"]);</script>
									</span>
								</label>
							</td>
							<td class="reControlCell">
								<input type="text" id="EmailSubject" />
							</td>
						</tr>
						<tr>
							<td class="reLabelCell">
								<label for="EmailCssClass" class="reDialogLabel">
									<span>
										<script type="text/javascript">document.write(localization["CssClass"]);</script>
									</span>
								</label>
							</td>
							<td class="reControlCell">
								<tools:ApplyClassDropDown ID="EmailCssClass" runat="server" />
							</td>
						</tr>
					</table>
				</telerik:RadPageView>
			<telerik:RadPageView ID="analyticsEventFieldset" runat="server">
			    <table border="0" cellpadding="0" cellspacing="0" class="reControlsLayout">
			        <tr>
			            <td class="reLabelCell">
			                <label for="CheckTriggerGoal" class="reDialogLabel">
			                    <span>
			                        Trigger Goal
			                    </span>
			                </label>
			            </td>
			            <td class="reControlCell">
			                <input type="checkbox" id="CheckTriggerGoal" />
			            </td>
			        </tr>
			        <tr>
			            <td class="reLabelCell">
			                <label for="ApplyGoalDropDown" class="reDialogLabel">
			                    <span>
			                        Goal
			                    </span>
			                </label>
			            </td>
			            <td class="reControlCell">
			                <select id="ApplyGoalDropDown">
			                    <option selected="selected">Select a Goal</option>
			                </select>
			            </td>
			        </tr>
			        <tr>
			            <td class="reLabelCell">
			                <label for="GoalDataValue" class="reDialogLabel">
			                    <span>
			                        Goal data value
			                    </span>
			                </label>
			            </td>
			            <td class="reControlCell">
			                <input type="text" id="GoalDataValue" />
			            </td>
			        </tr>
			    </table>
			</telerik:RadPageView>
			</telerik:radmultipage>
		</td>
	</tr>
	<asp:placeholder id="controlButtonsRow" runat="server">
	<tr>
		<td class="reBottomcell">
			<table border="0" cellpadding="0" cellspacing="0" class="reConfirmCancelButtonsTbl">
				<tr>
					<td>
						<button type="button" id="lmInsertButton">
							<script type="text/javascript">
                                setInnerHtml("lmInsertButton", localization["OK"]);
							</script>
						</button>
					</td>
					<td>
						<button type="button" id="lmCancelButton">
							<script type="text/javascript">
                                setInnerHtml("lmCancelButton", localization["Cancel"]);
							</script>
						</button>
					</td>
				</tr>
			</table>
		</td>
	</tr>
	</asp:placeholder>
</table>
<script type="text/JavaScript" src="/scripts/js/CustomHyperLinkManager.js"></script>
<script type="text/JavaScript" src="/scripts/js/TelerikHyperlinkManager.js"></script>

