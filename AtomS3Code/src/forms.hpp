#include <Arduino.h>
#include <M5ModuleDisplay.h>
#include <M5Unified.h>

#ifndef __InitializeComponent
#define __InitializeComponent

String getStringSplit(String str, char separator, int index)
{
    int len = str.length();
    int startPosition = 0;

    int separatorCount = 0;
    for (; startPosition < len; startPosition++)
    {
        if (separatorCount == index)
            break;

        if (str[startPosition] == separator)
            separatorCount++;
    }

    int endPosition = str.indexOf(separator, startPosition);

    if (!endPosition)
        endPosition = len - 1;

    return str.substring(startPosition, endPosition);
}

class form
{
public:
    int formWidth;
    int formHeight;

    bool formEnable;
    String formName;
    String valueString;
    M5Canvas Display_Main_Canvas;

    form()
    {
        formName = "BaseClass";
        formEnable = true;
    }

    form(M5Canvas formCanvas, int value)
    {
        formName = "BaseClass";
        formEnable = true;

        Display_Main_Canvas = formCanvas;
        formWidth = Display_Main_Canvas.width();
        formHeight = Display_Main_Canvas.height();
    }

    virtual void draw(float value, String text)
    {
    }
};

// =====================
// form_Top
// =====================
class form_Top : public form
{
private:
public:
    form_Top()
    {
        formName = "Top";
        formEnable = true;
    }

    /// @brief Initialize Canvas / Button
    form_Top(M5Canvas formCanvas, int value)
    {
        formName = "Top";
        formEnable = true;

        Display_Main_Canvas = formCanvas;
        formWidth = Display_Main_Canvas.width();
        formHeight = Display_Main_Canvas.height();
    }

    /// @brief Draw Canvas
    /// @param value
    /// @param text
    void draw(float value, String text) override
    {
        String Clock_String = getStringSplit(text, '\t', 0);
        String LengthON_String = getStringSplit(text, '\t', 1);
        String LengthOFF_String = getStringSplit(text, '\t', 2);
        String SwitchCount_String = getStringSplit(text, '\t', 3);
        String RunMode = getStringSplit(text, '\t', 4);
        String SwitchStatus_String = getStringSplit(text, '\t', 5);
        String DeviceInfo_String = getStringSplit(text, '\t', 6);
        String DeviceConnected_String = getStringSplit(text, '\t', 7);
        String EEPROMStatus_String = getStringSplit(text, '\t', 8);

        int LengthON = LengthON_String.toInt();
        int LengthOFF = LengthOFF_String.toInt();
        int LengthSUM = LengthON + LengthOFF;
        int SwitchCount = SwitchCount_String.toInt();

        int ONstart1 = (-SwitchCount * formWidth) / LengthSUM;
        int ONstart2 = ONstart1 + formWidth;
        int ONwidth = (LengthON * formWidth) / LengthSUM;

        if (formEnable)
        {
            /// refresh
            Display_Main_Canvas.fillScreen(BLACK);

            /// title
            Display_Main_Canvas.setFont(&fonts::DejaVu18);
            Display_Main_Canvas.setTextDatum(top_left);
            Display_Main_Canvas.setTextColor(0xffffff);
            Display_Main_Canvas.drawString("Int.Switch", 3, 0);
            Display_Main_Canvas.drawLine(0, 19, formWidth, 19, WHITE);
            Display_Main_Canvas.drawString(Clock_String, 3, 107);

            // On/Off status icon
            if (SwitchStatus_String == "ON")
            {
                Display_Main_Canvas.fillSmoothRoundRect(formWidth - 13, 3, 12, 12, 3, DARKGREEN);
            }
            else
            {
                Display_Main_Canvas.drawRoundRect(formWidth - 13, 3, 12, 12, 3, WHITE);
            }

            // Connection status icon
            if (DeviceConnected_String == "ON")
            {
                Display_Main_Canvas.fillSmoothRoundRect(formWidth - 13, formHeight - 15, 12, 12, 3, BLUE);
            }
            else
            {
                Display_Main_Canvas.drawRoundRect(formWidth - 13, formHeight - 15, 12, 12, 3, WHITE);
            }

            // EEPROM status icon
            if (EEPROMStatus_String == "initial")
            {
                Display_Main_Canvas.fillSmoothRoundRect(formWidth - 30, formHeight - 15, 12, 12, 3, YELLOW);
            }
            else if (EEPROMStatus_String == "saved")
            {
                Display_Main_Canvas.fillSmoothRoundRect(formWidth - 30, formHeight - 15, 12, 12, 3, RED);
            }
            else if (EEPROMStatus_String == "loaded")
            {
                Display_Main_Canvas.fillSmoothRoundRect(formWidth - 30, formHeight - 15, 12, 12, 3, GREENYELLOW);
            }
            else
            {
                Display_Main_Canvas.drawRoundRect(formWidth - 30, formHeight - 15, 12, 12, 3, WHITE);
            }

            // Run status bar
            int Position_StatusBarTop = 40;
            if (RunMode == "ON")
            {
                Display_Main_Canvas.fillRect(0, Position_StatusBarTop, formWidth, 20, DARKGREEN);
            }
            else if (RunMode == "OFF")
            {
                Display_Main_Canvas.drawRect(0, Position_StatusBarTop, formWidth, 20, 0xffffff);
            }
            else if (RunMode == "Auto")
            {
                Display_Main_Canvas.drawRect(0, Position_StatusBarTop, formWidth, 20, 0xffffff);
                Display_Main_Canvas.fillRect(ONstart1, Position_StatusBarTop, ONwidth, 20, DARKGREEN);
                Display_Main_Canvas.fillRect(ONstart2, Position_StatusBarTop, ONwidth, 20, DARKGREEN);

                Display_Main_Canvas.setTextDatum(top_center);
                Display_Main_Canvas.drawString(LengthON_String + "s", formWidth / 4, 20);
                Display_Main_Canvas.drawString("/", formWidth / 2, 20);
                Display_Main_Canvas.drawString(LengthOFF_String + "s", formWidth * 3 / 4, 20);
                Display_Main_Canvas.drawString(SwitchCount_String + "s", formWidth / 2, Position_StatusBarTop + 2);
            }

            Display_Main_Canvas.setTextDatum(top_center);
            Display_Main_Canvas.drawString(RunMode, formWidth / 2, Position_StatusBarTop + 22);

            Display_Main_Canvas.setFont(&fonts::DejaVu9);
            Display_Main_Canvas.setTextDatum(top_center);
            Display_Main_Canvas.drawString(DeviceInfo_String, formWidth / 2, formHeight - 20 * 2);

            Display_Main_Canvas.pushSprite(0, 0);
        }
    }
};

form *FormView;
form_Top Form_Top;

#endif
